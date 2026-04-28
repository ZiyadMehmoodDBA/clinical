using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Business.MedTextReferrals.ResponseModels
{
    public class ResponseModel
    {

    }
    
    [Serializable]
    public class patient
    {
        public long mdvision_id { get; set; }
        public string dob { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public phone phone { get; set; }
        public string zip_code { get; set; }
        public primary_insurance_provider primary_insurance_provider { get; set; }

        public patient()
        {
            primary_insurance_provider = new primary_insurance_provider();
            phone = new phone();
        }

    }
    public class phone
    {
        public string number { get; set; }
    }
    [Serializable]
    public class primary_insurance_provider
    {
        public long id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
    }
    [Serializable]
    public class location
    {
        public long id { get; set; }
        public string name { get; set; }
        public address address { get; set; }
        public location()
        {
            address = new address();
        }
    }
    [Serializable]
    public class seeing_doctor
    {
        public long id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string salutated_name { get; set; }
        public string npid { get; set; }
        public string phone { get; set; }
        public practice practice { get; set; }
        public default_location default_location { get; set; }
        public List<location> locations { get; set; }
        public List<specialty> specialties { get; set; }

        public seeing_doctor()
        {
            practice = new practice();
            default_location = new default_location();
            locations = new List<location>();
            specialties = new List<specialty>();
        }

    }
    [Serializable]
    public class practice
    {
        public long id { get; set; }
        public string name { get; set; }
    }
    [Serializable]
    public class specialty
    {
        public long id { get; set; }
        public string name { get; set; }
    }
    [Serializable]
    public class address
    {
        public long id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string zip { get; set; }
    }
    [Serializable]
    public class default_location
    {
        public string id { get; set; }
        public string name { get; set; }
        public address address { get; set; }
        public default_location()
        {
            address = new address();
        }
    }
    [Serializable]
    public class checkIn
    {
        public long id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string status { get; set; }
        public string source { get; set; }
        public patient patient { get; set; }
        public location location { get; set; }
        public seeing_doctor seeing_doctor { get; set; }
        public string human_status { get; set; }
        public string data_url { get; set; }
        public List<object> recent_leads { get; set; }
        public List<object> leads { get; set; }
        public object referring_doctor { get; set; }

        public checkIn()
        {
            patient = new patient();
            location = new location();
            seeing_doctor = new seeing_doctor();
            recent_leads = new List<object>();
            leads = new List<object>();
            referring_doctor = new object();
        }

    }
    [Serializable]
    public class checkInResponse
    {
        public bool IsCheckedIn { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseString { get; set; }
        public string CheckOutURL { get; set; }
        public checkIn checkIn { get; set; }
        public checkInResponse()
        {
            checkIn = new checkIn();
            IsCheckedIn = false;
        }
    }
}
