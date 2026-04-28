using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.Reports
{
    public class CImmunizationModel
    {
        public bool IncludeInactivePatient { get; set; }
        public bool IsAdministatered { get; set; }
        
        public string AccountNumber { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string Provider { get; set; }
        public string VaccineCategory { get; set; }
        public string Vaccine { get; set; }
        public string Route { get; set; }
        public string Site { get; set; }
        public string ImmunizationReaction { get; set; }
        public string ImmunizationAlert { get; set; }
        public string ImmunizationToDate { get; set; }
        public string ImmunizationFromDate { get; set; }
        public bool voidDose { get; set; }
        
        
    }
    public class CImmunizationFillModel {
        public string AccountNumber { get; set; }

        public string PatientName { get; set; }

        public string DOB { get; set; }

        public string PatStatus { get; set; }
        public string vaccine { get; set; }
        public string Alert { get; set; }
        public string Route { get; set; }
        public string Site { get; set; }
        public string Reaction { get; set; }
        public string AdministeredBy { get; set; }
        public string AdminDate { get; set; }
        public string DueDate { get; set; }
        public long PatientId { get; set; }
        public string Category { get; set; }
        public string VoidDose { get; set; }
        
    
    }


}
