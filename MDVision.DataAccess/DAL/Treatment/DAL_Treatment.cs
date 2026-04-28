using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.DataAccess.DCommon;
using System.ComponentModel;
using MDVision.Datasets;
using MDVision.Model.Clinical.Reports;
using System.Data.SqlClient;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Clinical.Notes;
using MDVision.Model.Clinical.Immunization;
using MDVision.Common.Utilities;
using MDVision.Model.Common;
using MDVision.Model.Clinical.LegacyNotes;
using MDVision.Model.Clinical.Orderset;
using MDVision.Model.Clinical.Treatment;
using MDVision.Model.Clinical.Medication;
using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;

namespace MDVision.DataAccess.DAL.Treatment
{
    public class DAL_Treatment
    {
        #region "Stored Procedure Names"
        private const string PROC_TREATMENT_INSERT = "Clinical.sp_TreatmentInsertUpdate";
        private const string PROC_SELECT_TREATMENT_DATA = "Clinical.Sp_SelectTreatmentData";
        private const string PROC_SELECT_TREATMENT_DATA_MAPPING = "Clinical.Sp_SelectTreatmentMapping";
        private const string PROC_DETACH_TREATMENT_FROM_NOTES = "Clinical.Sp_DetachTreatmentFromNotes";
        private const string PROC_SELECT_PREV_NOTES_TREATMENT_COMMENTS = "Clinical.sp_SelectPrevNoteTeatmentComments";
        #endregion

        #region "Parameters"
        private const string PARM_XML = "@XML";
        private const string PARAM_CREATED_BY = "@CreatedBy";
        private const string PARAM_CREATED_ON = "@CreatedOn";
        private const string PARAM_MODIFIED_ON = "@ModifiedOn";
        private const string PARAM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_NOTES_ID = "@NoteId";
        private const string PARAM_COMMENT = "@Comment";
        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_DELETE_TREATMENT = "@DeleteTreatmentId";
        private const string PARM_USER_ID = "@UserId";
        private const string PARM_PATIENT_ID = "@PatientId";
        private const string PARM_PROVIDER_ID = "@ProviderId";

        #endregion

        #region Constructors

        public DAL_Treatment()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
        }

        public DAL_Treatment(SharedVariable SharedVariable)
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject(SharedVariable);
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



        public TreatementSoapTextDataModel SaveTreatment(Int64 NoteId, string XML, string Comment, DataTable dtDeleteTreatmentsIds, string CreatedBy, DateTime CreatedOn, string ModifiedBy, DateTime ModifiedOn)
        {
            SqlDataReader reader = null;
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {

                dbManager.BeginTransaction();
                dbManager.CreateParameters(9);
                dbManager.AddParameters(0, PARM_NOTES_ID, NoteId);
                dbManager.AddParameters(1, PARM_XML, XML);
                dbManager.AddParameters(2, PARAM_CREATED_BY, CreatedBy);
                dbManager.AddParameters(3, PARAM_CREATED_ON, CreatedOn);
                dbManager.AddParameters(4, PARAM_MODIFIED_ON, ModifiedOn);
                dbManager.AddParameters(5, PARAM_MODIFIED_BY, ModifiedBy);
                dbManager.AddParameters(6, PARAM_COMMENT, Comment);
                dbManager.AddParameters(7, PARM_ENTITY_ID, MDVSession.Current.EntityId);
                dbManager.AddParameters(8, PARM_DELETE_TREATMENT, dtDeleteTreatmentsIds);


                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_TREATMENT_INSERT);
                TreatementSoapTextDataModel result = new TreatementSoapTextDataModel();

                result.TreatmentList = new List<Treatments>();
                result.TreatmentDataList = new List<TreatmentData>();
                result.TreatementPrescriptionList = new List<ClinicalPrescriptionModel>();
                result.TreatmentProcedureList = new List<TreatmentProcedure>();
                result.LabOrderList = new List<LabOrderModel>();
                result.LabOrderTestList = new List<LabOrderTestModel>();
                result.LabOrderProblemList = new List<LabOrderProblemModel>();
                result.RadiologyOrderList = new List<RadiologyOrderModel>();
                result.RadiologyOrderTestList = new List<RadiologyOrderTestModel>();
                result.RadiologyOrderProblemList = new List<RadiologyOrderProblemModel>();
                result.ReferralList = new List<ReferralModel>();
                result.ReferralProcedureList = new List<ReferralProcedureModel>();
                result.ReferralProblemList = new List<ReferralProblemModel>();
                result.VaccineHxList = new List<TreatmentVaccineHx>();
                result.TerapeuticList = new List<TreatmentTerapuetic>();
                while (reader.Read())
                {
                    Treatments Treatment = new Treatments();
                    Treatment.TreatmentId = Convert.ToString(reader["TreatmentId"]);
                    Treatment.NoteId = Convert.ToString(reader["NoteId"]);
                    Treatment.ProblemListId = Convert.ToString(reader["ProblemListId"]);
                    Treatment.ProblemDescription = Convert.ToString(reader["ProblemDescription"]);
                    Treatment.Comments = Convert.ToString(reader["Comments"]);
                    result.TreatmentList.Add(Treatment);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    TreatmentData Treatmentdata = new TreatmentData();
                    Treatmentdata.TreatmentDataId = Convert.ToString(reader["TreatmentDataId"]);
                    Treatmentdata.TreatmentId = Convert.ToString(reader["TreatmentId"]);
                    Treatmentdata.ComponentId = Convert.ToString(reader["ComponentId"]);
                    Treatmentdata.ComponentName = Convert.ToString(reader["ComponentName"]);
                    Treatmentdata.DataId = Convert.ToString(reader["DataId"]);
                    result.TreatmentDataList.Add(Treatmentdata);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClinicalPrescriptionModel TreatementPrescription = new ClinicalPrescriptionModel();

                    TreatementPrescription.PrescriptionID = Convert.ToInt64(reader["PrescriptionID"]);
                    TreatementPrescription.MedicationName = Convert.ToString(reader["MedicationName"]);
                    TreatementPrescription.ProviderName = Convert.ToString(reader["ProviderName"]);
                    TreatementPrescription.CreatedDate = Convert.ToString(reader["CreatedDate"]);
                    TreatementPrescription.Refill = Convert.ToString(reader["Refill"]);
                    TreatementPrescription.Action = Convert.ToString(reader["Action"]);
                    TreatementPrescription.Dose = Convert.ToString(reader["Dose"]);
                    TreatementPrescription.DoseUnit = Convert.ToString(reader["DoseUnit"]);
                    TreatementPrescription.Routeby = Convert.ToString(reader["Routeby"]);
                    TreatementPrescription.DoseTiming = Convert.ToString(reader["DoseTiming"]);
                    TreatementPrescription.Duration = Convert.ToString(reader["Duration"]);
                    TreatementPrescription.Quantity = Convert.ToString(reader["Quantity"]);
                    TreatementPrescription.QuantityUnit = Convert.ToString(reader["QuantityUnit"]);
                    TreatementPrescription.Substitution = Convert.ToString(reader["Substitution"]);
                    TreatementPrescription.NDCID = Convert.ToString(reader["NDCID"]);
                    result.TreatementPrescriptionList.Add(TreatementPrescription);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    TreatmentProcedure TreatementProcedure = new TreatmentProcedure();
                    TreatementProcedure.ProcedureId = Convert.ToString(reader["ProcedureId"]);
                    TreatementProcedure.StartDate = Convert.ToString(reader["StartDate"]);
                    TreatementProcedure.EndDate = Convert.ToString(reader["EndDate"]);
                    TreatementProcedure.Comments = Convert.ToString(reader["Comments"]);
                    TreatementProcedure.Diagnosis = Convert.ToString(reader["Diagnosis"]);
                    TreatementProcedure.ProcedureCodeName = Convert.ToString(reader["ProcedureCodeName"]);
                    TreatementProcedure.ProcedureTemplateSoapTextExists = Convert.ToString(reader["ProcedureTemplateSoapTextExists"]);
                    result.TreatmentProcedureList.Add(TreatementProcedure);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    LabOrderModel TreatementLabOrder = new LabOrderModel();
                    TreatementLabOrder.LabOrderId = Convert.ToString(reader["LabOrderId"]);
                    TreatementLabOrder.PatientId = Convert.ToString(reader["PatientId"]);
                    TreatementLabOrder.LabId = Convert.ToString(reader["LabId"]);
                    TreatementLabOrder.FacilityId = Convert.ToString(reader["FacilityId"]);
                    TreatementLabOrder.FacilityShortName = Convert.ToString(reader["FacilityShortName"]);
                    TreatementLabOrder.ProviderId = Convert.ToString(reader["ProviderId"]);
                    TreatementLabOrder.ProviderFirstName = Convert.ToString(reader["ProviderFirstName"]);
                    TreatementLabOrder.ProviderLastName = Convert.ToString(reader["ProviderLastName"]);
                    TreatementLabOrder.AssigneeId = Convert.ToString(reader["AssigneeId"]);
                    TreatementLabOrder.AssigneeFirstName = Convert.ToString(reader["AssigneeFirstName"]);
                    TreatementLabOrder.AssigneeLastName = Convert.ToString(reader["AssigneeLastName"]);
                    TreatementLabOrder.OrderDate = Convert.ToString(reader["OrderDate"]);
                    TreatementLabOrder.OrderTime = Convert.ToString(reader["OrderTime"]);
                    TreatementLabOrder.BillingTypeId = Convert.ToString(reader["BillingTypeId"]);
                    TreatementLabOrder.PrimaryInsuraceId = Convert.ToString(reader["PrimaryInsuraceId"]);
                    TreatementLabOrder.PrimaryInsuraceFirstName = Convert.ToString(reader["PrimaryInsuraceFirstName"]);
                    TreatementLabOrder.PrimaryInsuraceLastName = Convert.ToString(reader["PrimaryInsuraceLastName"]);
                    TreatementLabOrder.SecondaryInsuraceId = Convert.ToString(reader["SecondaryInsuraceId"]);
                    TreatementLabOrder.SecondaryInsuraceFirstName = Convert.ToString(reader["SecondaryInsuraceFirstName"]);
                    TreatementLabOrder.SecondaryInsuraceLastName = Convert.ToString(reader["SecondaryInsuraceLastName"]);
                    TreatementLabOrder.TertiaryInsuraceId = Convert.ToString(reader["TertiaryInsuraceId"]);
                    TreatementLabOrder.TertiaryInsuraceFirstName = Convert.ToString(reader["TertiaryInsuraceFirstName"]);
                    TreatementLabOrder.TertiaryInsuraceLastName = Convert.ToString(reader["TertiaryInsuraceLastName"]);
                    TreatementLabOrder.RelationShipId = Convert.ToString(reader["RelationShipId"]);
                    TreatementLabOrder.IsActive = Convert.ToString(reader["IsActive"]);
                    TreatementLabOrder.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    TreatementLabOrder.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    TreatementLabOrder.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    TreatementLabOrder.ModifiedBy = Convert.ToString(reader["CreatedOn"]);
                    TreatementLabOrder.SoapText = Convert.ToString(reader["SoapText"]);
                    TreatementLabOrder.OrderNo = Convert.ToString(reader["OrderNo"]);
                    TreatementLabOrder.Status = Convert.ToString(reader["Status"]);
                    result.LabOrderList.Add(TreatementLabOrder);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    LabOrderTestModel TreatementLabOrderTest = new LabOrderTestModel();
                    TreatementLabOrderTest.LabOrderTestId = Convert.ToString(reader["LabOrderTestId"]);
                    TreatementLabOrderTest.LabOrderId = Convert.ToString(reader["LabOrderId"]);
                    TreatementLabOrderTest.UrgencyName = Convert.ToString(reader["UrgencyName"]);
                    TreatementLabOrderTest.CollectedAtName = Convert.ToString(reader["CollectedAtName"]);
                    TreatementLabOrderTest.Volume = Convert.ToString(reader["Volume"]);
                    TreatementLabOrderTest.CPTCode = Convert.ToString(reader["CPTCode"]);
                    TreatementLabOrderTest.CPTCodeDescription = Convert.ToString(reader["CPTCodeDescription"]);
                    TreatementLabOrderTest.IsActive = Convert.ToString(reader["IsActive"]);
                    TreatementLabOrderTest.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    TreatementLabOrderTest.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    TreatementLabOrderTest.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    TreatementLabOrderTest.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    TreatementLabOrderTest.ShowCPTCode = Convert.ToString(reader["ShowCPTCode"]);
                    result.LabOrderTestList.Add(TreatementLabOrderTest);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    LabOrderProblemModel TreatementLabOrderproblem = new LabOrderProblemModel();
                    TreatementLabOrderproblem.LabOrderProblemId = Convert.ToString(reader["LabOrderProblemId"]);
                    TreatementLabOrderproblem.LabOrderId = Convert.ToString(reader["LabOrderId"]);
                    TreatementLabOrderproblem.IsActive = Convert.ToString(reader["IsActive"]);
                    TreatementLabOrderproblem.SoapText = Convert.ToString(reader["SoapText"]);
                    TreatementLabOrderproblem.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    TreatementLabOrderproblem.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    TreatementLabOrderproblem.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    TreatementLabOrderproblem.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    result.LabOrderProblemList.Add(TreatementLabOrderproblem);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    RadiologyOrderModel TreatementRadiologyOrder = new RadiologyOrderModel();
                    TreatementRadiologyOrder.RadiologyOrderId = Convert.ToString(reader["RadiologyOrderId"]);
                    TreatementRadiologyOrder.PatientId = Convert.ToString(reader["PatientId"]);
                    TreatementRadiologyOrder.LabId = Convert.ToString(reader["LabId"]);
                    TreatementRadiologyOrder.FacilityId = Convert.ToString(reader["FacilityId"]);
                    TreatementRadiologyOrder.FacilityShortName = Convert.ToString(reader["FacilityShortName"]);
                    TreatementRadiologyOrder.ProviderId = Convert.ToString(reader["ProviderId"]);
                    TreatementRadiologyOrder.ProviderFirstName = Convert.ToString(reader["ProviderFirstName"]);
                    TreatementRadiologyOrder.ProviderLastName = Convert.ToString(reader["ProviderLastName"]);
                    TreatementRadiologyOrder.AssigneeId = Convert.ToString(reader["AssigneeId"]);
                    TreatementRadiologyOrder.AssigneeFirstName = Convert.ToString(reader["AssigneeFirstName"]);
                    TreatementRadiologyOrder.AssigneeLastName = Convert.ToString(reader["AssigneeLastName"]);
                    TreatementRadiologyOrder.OrderDate = Convert.ToString(reader["OrderDate"]);
                    TreatementRadiologyOrder.OrderTime = Convert.ToString(reader["OrderTime"]);
                    TreatementRadiologyOrder.BillingTypeId = Convert.ToString(reader["BillingTypeId"]);
                    TreatementRadiologyOrder.PrimaryInsuraceId = Convert.ToString(reader["PrimaryInsuraceId"]);
                    TreatementRadiologyOrder.SecondaryInsuraceId = Convert.ToString(reader["SecondaryInsuraceId"]);
                    TreatementRadiologyOrder.TertiaryInsuraceId = Convert.ToString(reader["TertiaryInsuraceId"]);
                    TreatementRadiologyOrder.PrimaryInsuraceFirstName = Convert.ToString(reader["PrimaryInsuraceFirstName"]);
                    TreatementRadiologyOrder.PrimaryInsuraceLastname = Convert.ToString(reader["PrimaryInsuraceLastname"]);
                    TreatementRadiologyOrder.SecondaryInsuraceFirstName = Convert.ToString(reader["SecondaryInsuraceFirstName"]);
                    TreatementRadiologyOrder.SecondaryInsuraceLastName = Convert.ToString(reader["SecondaryInsuraceLastName"]);
                    TreatementRadiologyOrder.TertiaryInsuraceFirstName = Convert.ToString(reader["TertiaryInsuraceFirstName"]);
                    TreatementRadiologyOrder.TertiaryInsuraceLastName = Convert.ToString(reader["TertiaryInsuraceLastName"]);
                    TreatementRadiologyOrder.RelationCode = Convert.ToString(reader["RelationCode"]);
                    TreatementRadiologyOrder.RelationDescription = Convert.ToString(reader["RelationDescription"]);
                    TreatementRadiologyOrder.GuarantorId = Convert.ToString(reader["GuarantorId"]);
                    TreatementRadiologyOrder.GuarantorFirstName = Convert.ToString(reader["GuarantorFirstName"]);
                    TreatementRadiologyOrder.GuarantorLastName = Convert.ToString(reader["GuarantorLastName"]);
                    TreatementRadiologyOrder.RelationShipId = Convert.ToString(reader["RelationShipId"]);
                    TreatementRadiologyOrder.IsActive = Convert.ToString(reader["IsActive"]);
                    TreatementRadiologyOrder.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    TreatementRadiologyOrder.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    TreatementRadiologyOrder.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    TreatementRadiologyOrder.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    TreatementRadiologyOrder.SoapText = Convert.ToString(reader["SoapText"]);
                    TreatementRadiologyOrder.OrderNo = Convert.ToString(reader["OrderNo"]);
                    TreatementRadiologyOrder.Status = Convert.ToString(reader["Status"]);
                    TreatementRadiologyOrder.Comments = Convert.ToString(reader["Comments"]);
                    result.RadiologyOrderList.Add(TreatementRadiologyOrder);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    RadiologyOrderTestModel RadiologyOrderTestList = new RadiologyOrderTestModel();
                    RadiologyOrderTestList.RadiologyOrderTestId = Convert.ToString(reader["RadiologyOrderTestId"]);
                    RadiologyOrderTestList.RadiologyOrderId = Convert.ToString(reader["RadiologyOrderId"]);
                    RadiologyOrderTestList.UrgencyName = Convert.ToString(reader["UrgencyName"]);
                    RadiologyOrderTestList.CollectedAtName = Convert.ToString(reader["CollectedAtName"]);
                    RadiologyOrderTestList.Specimen = Convert.ToString(reader["Specimen"]);
                    RadiologyOrderTestList.Volume = Convert.ToString(reader["Volume"]);
                    RadiologyOrderTestList.CPTCode = Convert.ToString(reader["CPTCode"]);
                    RadiologyOrderTestList.CPTCodeDescription = Convert.ToString(reader["CPTCodeDescription"]);
                    RadiologyOrderTestList.IsActive = Convert.ToString(reader["IsActive"]);
                    RadiologyOrderTestList.ShowCptCode = Convert.ToString(reader["ShowCptCode"]);
                    RadiologyOrderTestList.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    RadiologyOrderTestList.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    RadiologyOrderTestList.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    RadiologyOrderTestList.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    result.RadiologyOrderTestList.Add(RadiologyOrderTestList);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    RadiologyOrderProblemModel RadiologyOrderProbelmList = new RadiologyOrderProblemModel();
                    RadiologyOrderProbelmList.RadiologyOrderProblemId = Convert.ToString(reader["RadiologyOrderProblemId"]);
                    RadiologyOrderProbelmList.RadiologyOrderId = Convert.ToString(reader["RadiologyOrderId"]);
                    RadiologyOrderProbelmList.IsActive = Convert.ToString(reader["IsActive"]);
                    RadiologyOrderProbelmList.SoapText = Convert.ToString(reader["SoapText"]);
                    RadiologyOrderProbelmList.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    RadiologyOrderProbelmList.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    RadiologyOrderProbelmList.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    RadiologyOrderProbelmList.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    result.RadiologyOrderProblemList.Add(RadiologyOrderProbelmList);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ReferralModel Referral = new ReferralModel();

                    Referral.ReferralId = Convert.ToString(reader["ReferralId"]);
                    Referral.PatientId = Convert.ToString(reader["PatientId"]);
                    Referral.Type = Convert.ToString(reader["Type"]);
                    Referral.ProviderId = Convert.ToString(reader["ProviderId"]);
                    Referral.AssigneeId = Convert.ToString(reader["AssigneeId"]);
                    Referral.RefProviderId = Convert.ToString(reader["RefProviderId"]);
                    Referral.ProviderName = Convert.ToString(reader["ProviderName"]);
                    Referral.AssigneeName = Convert.ToString(reader["AssigneeName"]);
                    Referral.Date = Convert.ToString(reader["Date"]);
                    Referral.Time = Convert.ToString(reader["Time"]);
                    Referral.IsActive = Convert.ToString(reader["IsActive"]);
                    Referral.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    Referral.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    Referral.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    Referral.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    Referral.SoapText = Convert.ToString(reader["SoapText"]);
                    Referral.Status = Convert.ToString(reader["Status"]);
                    Referral.PAN = Convert.ToString(reader["PAN"]);
                    Referral.Visits = Convert.ToString(reader["Visits"]);
                    Referral.Reason = Convert.ToString(reader["Reason"]);
                    Referral.DateFrom = Convert.ToString(reader["DateFrom"]);
                    Referral.DateTo = Convert.ToString(reader["DateTo"]);
                    Referral.PatientInsuranceName = Convert.ToString(reader["PatientInsuranceName"]);
                    Referral.RefProviderName = Convert.ToString(reader["RefProviderName"]);
                    Referral.FacilityFromName = Convert.ToString(reader["FacilityFromName"]);
                    Referral.FacilityToName = Convert.ToString(reader["FacilityToName"]);
                    Referral.SpecialityToName = Convert.ToString(reader["SpecialityToName"]);
                    Referral.StatusName = Convert.ToString(reader["StatusName"]);
                    Referral.Procedures = Convert.ToString(reader["Procedures"]);
                    Referral.Comments = Convert.ToString(reader["Comments"]);
                    result.ReferralList.Add(Referral);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ReferralProcedureModel ReferralProcedure = new ReferralProcedureModel();
                    ReferralProcedure.ReferralProcedureId = Convert.ToString(reader["ReferralProcedureId"]);
                    ReferralProcedure.ReferralId = Convert.ToString(reader["ReferralId"]);
                    ReferralProcedure.CPTCode = Convert.ToString(reader["CPTCode"]);
                    ReferralProcedure.CPTCodeDescription = Convert.ToString(reader["CPTCodeDescription"]);
                    ReferralProcedure.Procedure = Convert.ToString(reader["Procedure"]);
                    ReferralProcedure.UrgencyId = Convert.ToString(reader["UrgencyId"]);
                    ReferralProcedure.Urgency = Convert.ToString(reader["Urgency"]);
                    ReferralProcedure.UrgencyName = Convert.ToString(reader["UrgencyName"]);
                    ReferralProcedure.ShowCptCode = Convert.ToString(reader["ShowCptCode"]);
                    //ReferralProcedure.Comments = Convert.ToString(reader["Comments"]);
                    ReferralProcedure.IsActive = Convert.ToString(reader["IsActive"]);
                    ReferralProcedure.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    ReferralProcedure.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    ReferralProcedure.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    ReferralProcedure.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    result.ReferralProcedureList.Add(ReferralProcedure);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ReferralProblemModel ReferralProblem = new ReferralProblemModel();
                    ReferralProblem.ReferralProblemId = Convert.ToString(reader["ReferralProblemId"]);
                    ReferralProblem.ReferralId = Convert.ToString(reader["ReferralId"]);
                    ReferralProblem.ProblemId = Convert.ToString(reader["ProblemId"]);
                    ReferralProblem.ProblemName = Convert.ToString(reader["ProblemName"]);
                    ReferralProblem.Comments = Convert.ToString(reader["Comments"]);
                    ReferralProblem.SoapText = Convert.ToString(reader["SoapText"]);
                    ReferralProblem.IsActive = Convert.ToString(reader["IsActive"]);
                    ReferralProblem.ModifiedOn = Convert.ToString(reader["ModifiedOn"]);
                    ReferralProblem.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                    ReferralProblem.CreatedOn = Convert.ToString(reader["CreatedOn"]);
                    ReferralProblem.ModifiedBy = Convert.ToString(reader["ModifiedBy"]);
                    result.ReferralProblemList.Add(ReferralProblem);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    TreatmentVaccineHx VaccineHx = new TreatmentVaccineHx();
                    VaccineHx.VaccineHxId = Convert.ToString(reader["VaccineHxId"]);
                    VaccineHx.VaccineName = Convert.ToString(reader["VaccineName"]);
                    VaccineHx.Dose = Convert.ToString(reader["Dose"]);
                    VaccineHx.AdministrationDate = Convert.ToString(reader["AdministrationDate"]);
                    VaccineHx.Amount = Convert.ToString(reader["Amount"]);
                    VaccineHx.RouteDescription = Convert.ToString(reader["RouteDescription"]);
                    VaccineHx.SiteDescription = Convert.ToString(reader["SiteDescription"]);
                    VaccineHx.ManufacturerName = Convert.ToString(reader["ManufacturerName"]);
                    VaccineHx.ExpiryDate = Convert.ToString(reader["ExpiryDate"]);
                    VaccineHx.Type = Convert.ToString(reader["Type"]);
                    VaccineHx.VoidDose = Convert.ToString(reader["VoidDose"]).ToLower() == "false" ? false : true;
                    VaccineHx.ProviderName = Convert.ToString(reader["ProviderName"]);
                    VaccineHx.RefusalReason = Convert.ToString(reader["RefusalReason"]);
                    VaccineHx.Comments = Convert.ToString(reader["Comments"]);
                    VaccineHx.CPT = Convert.ToString(reader["CPT"]);
                    result.VaccineHxList.Add(VaccineHx);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    TreatmentTerapuetic Terapuetic = new TreatmentTerapuetic();
                    Terapuetic.Type = Convert.ToString(reader["Type"]);
                    Terapuetic.CPTCode = Convert.ToString(reader["CPTCode"]);
                    Terapuetic.TherapeuticInjection = Convert.ToString(reader["TherapeuticInjection"]);
                    Terapuetic.ImmTherInjectionId = Convert.ToString(reader["ImmTherInjectionId"]);
                    Terapuetic.Dose = !string.IsNullOrEmpty(reader["Dose"].ToString()) ? MDVUtility.Tofloat(reader["Dose"]) : 0;
                    Terapuetic.AdministrationDate = Convert.ToString(reader["AdministrationDate"]);
                    Terapuetic.Amount = Convert.ToString(reader["Amount"]);
                    Terapuetic.RouteDescription = Convert.ToString(reader["RouteDescription"]);
                    Terapuetic.SiteDescription = Convert.ToString(reader["SiteDescription"]);
                    Terapuetic.ManufacturerName = Convert.ToString(reader["ManufacturerName"]);
                    Terapuetic.ProviderName = Convert.ToString(reader["ProviderName"]);
                    result.TerapeuticList.Add(Terapuetic);
                }
                if (reader != null)
                    reader.Close();
                dbManager.CommitTransaction();
                return result;
            }
            catch (Exception ex)
            {
                dbManager.RollBackTransaction();
                MDVLogger.DALErrorLog("DAL_Treatment::SaveTreatment", PROC_TREATMENT_INSERT, ex);
                throw ex;
            }
            finally
            {


                dbManager.Dispose();
            }
        }

        public TreatementSoapTextDataModel GetTreatmentData(long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_NOTES_ID, NotesId);
                var resultSet = dbManager.ExecuteReadersForMultiResultSets<TreatementSoapTextDataModel>(PROC_SELECT_TREATMENT_DATA, typeof(Treatments), typeof(TreatmentData), typeof(ClinicalPrescriptionModel), typeof(TreatmentProcedure));
                return resultSet;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTreatment::GetTreatmentData", PROC_SELECT_TREATMENT_DATA, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public TreatementSoapTextDataModel GetTreatmentMapping(long NotesId)
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.AddParameters(PARM_NOTES_ID, NotesId);
                var resultSet = dbManager.ExecuteReadersForMultiResultSets<TreatementSoapTextDataModel>(PROC_SELECT_TREATMENT_DATA_MAPPING, typeof(Treatments), typeof(TreatmentData));
                return resultSet;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTreatment::GetTreatmentMapping", PROC_SELECT_TREATMENT_DATA_MAPPING, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public string detachTreatment(long NotesId)
        {

            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(1);

                dbManager.AddParameters(0, PARM_NOTES_ID, NotesId);


                dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_DETACH_TREATMENT_FROM_NOTES);
                return "";
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTreatment::detachTreatment", PROC_DETACH_TREATMENT_FROM_NOTES, ex);
                string[] str = ex.Message.Split('|');
                if (str.Length > 1)
                    return str[1].ToString();
                else
                    return ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public TreatmentPlanCommentModel GetPrevTreatmentPlanComments(long patientId, long userId, long entityId, long providerId)
        {
            TreatmentPlanCommentModel result = new TreatmentPlanCommentModel();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0,PARM_PATIENT_ID, patientId);
                dbManager.AddParameters(1,PARM_USER_ID, userId);
                dbManager.AddParameters(2,PARM_ENTITY_ID, entityId);
                dbManager.AddParameters(3,PARM_PROVIDER_ID, providerId);
                SqlDataReader reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_SELECT_PREV_NOTES_TREATMENT_COMMENTS);
          

                while (reader.Read())
                {
                    result.SoapText = Convert.ToString(reader["SoapText"]);
                }
             
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DAL_Treatment::GetPrevTreatmentPlanComments", PROC_SELECT_PREV_NOTES_TREATMENT_COMMENTS, ex);
            }
            finally
            {
                dbManager.Dispose();
            }
            return result;
        }
    }
}
