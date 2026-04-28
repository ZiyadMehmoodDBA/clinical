using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Business.MedTextReferrals.RequestModels
{
    public class RequestModel
    {

    }

    public class mdvision_medtext
    {
        public mdvision_appointment mdvision_appointment { get; set; }
        public mdvision_medtext()
        {
            mdvision_appointment = new mdvision_appointment();
        }
    }

    [Serializable]
    public class mdvision_appointment
    {

        public long mdvision_id { get; set; }
        public patient patient { get; set; }

        public attending_provider attending_provider { get; set; }
        public referring_provider referring_provider { get; set; }

        public location location { get; set; }
        public List<referrals> referrals { get; set; }

        public mdvision_appointment()
        {
            attending_provider = new attending_provider();
            referring_provider = new referring_provider();
            location = new location();
            patient = new patient();
            referrals = new List<referrals>();
        }

    }
    [Serializable]
    public class patient
    {
        public long mdvision_id { get; set; }
        public string dob { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string zip_code { get; set; }
        public string account_number { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public address address { get; set; }
        public primary_insurance_provider primary_insurance_provider { get; set; }
        public secondary_insurance secondary_insurance { get; set; }
        public List<problems> problems { get; set; }
        public List<procedures> procedures { get; set; }

        public patient()
        {
            primary_insurance_provider = new primary_insurance_provider();
            secondary_insurance = new secondary_insurance();
            procedures = new List<procedures>();
            problems = new List<problems>();
            address = new address();
        }

    }

    [Serializable]
    public class address
    {
        public string street { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    [Serializable]
    public class problems
    {
        public string code { get; set; }
        public string desc { get; set; }
        
    }
    [Serializable]
    public class procedures
    {
        public string code { get; set; }
        public string desc { get; set; }
        public string urgency { get; set; }
    }  
    [Serializable]
    public class primary_insurance_provider
    {
        public string name { get; set; }
    }
    [Serializable]
    public class secondary_insurance
    {
        public string name { get; set; }
    }
    [Serializable]
    public class attending_provider
    {
        public string npi { get; set; }
    }
    [Serializable]
    public class referring_provider
    {
        public string npi { get; set; }
    }
    [Serializable]
    public class location
    {
        public long id { get; set; }
    }

    [Serializable]
    public class receiving_provider
    {
        public string npi { get; set; }
    }
    [Serializable]
    public class referrals
    {
        public receiving_provider receiving_provider { get; set; }
        public string visit_type { get; set; }
        public string reason { get; set; }
        public string notes { get; set; }

        public referrals()
        {
            receiving_provider = new receiving_provider();
        }
    }
}
