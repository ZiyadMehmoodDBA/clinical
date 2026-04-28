using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class LegacyNotesViewModel
    {
        public List<Complaints> Complaints { get; set; }
        public List<SocialHx> SocialHx { get; set; }
        public List<MedicalHx> MedicalHx { get; set; }
        public List<BirthHx> BirthHx { get; set; }
        public List<AllergyHx> AllergyHx { get; set; }
        public List<MedicationHx> MedicationHx { get; set; }
        public List<ImmunizationHx> ImmunizationHx { get; set; }
        public List<ProblemHx> ProblemHx { get; set; }
        public List<FamilyHx> FamilyHx { get; set; }
        public List<SurgicalHx> SurgicalHx { get; set; }
        public List<HospitalizationHx> HospitalizationHx { get; set; }
        public List<VitalSigns> VitalSigns { get; set; }
        public List<LabOrder> LabOrder { get; set; }
        public List<LabOrderResult> LabOrderResult { get; set; }
        public List<RadOrder> RadOrder { get; set; }
        public List<RadOrderResult> RadOrderResult { get; set; }
        public List<Procedure> Procedure { get; set; }
        public List<Referrals> Referrals { get; set; }
        public List<ProcedureOrder> ProcedureOrder { get; set; }
        public List<ConsultationOrder> ConsultationOrder { get; set; }
        public List<FollowUp> FollowUp { get; set; }
        public List<FunctionalCognitive> FunctionalCognitive { get; set; }
        public List<PatientEducation> PatientEducation { get; set; }
        public List<PhysicalExam> PhysicalExam { get; set; }
        public List<eSuperbill> eSuperbill { get; set; }
        public List<NotesComponent> NotesComponent { get; set; }
        public List<Prescription> Prescription { get; set; }
        public List<NoteHeaderData> NoteHeaderData { get; set; }
        public List<PatientLetter> PatientLetter { get; set; }

    }
}
