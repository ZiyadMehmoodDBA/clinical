using MDVision.Business.AppointmentReminders.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MDVision.Business.AppointmentReminders.Request
{
    public class request
    {
        public string key { get; set; }
        public string preference { get; set; }
        public status status { get; set; }

        public text text { get; set; }
        public call call { get; set; }

        public fax fax { get; set; }

        public email email { get; set; }

        public request()
        {
            this.status = new status();
            this.fax = new fax();
            this.text = new text();
            this.call = new call();
            this.email = new email();
        }

        public request(string key)
        {
            this.key = key;
            this.status = new status();
            this.fax = new fax();
            this.text = new text();
            this.call = new call();
            this.email = new email();
        }
    }
    [XmlRoot("status")]
    public class status
    {
        [XmlElement("call")]
        public List<string> call { get; set; }
        [XmlElement("text")]
        public List<string> text { get; set; }
        [XmlElement("email")]
        public List<string> email { get; set; }

        public status() {
            this.call = new List<string>();
            this.text = new List<string>();
            this.email = new List<string>();
        }
    }

    public class call
    {
        public string id { get; set; }
        public string status { get; set; }
        public string delivery { get; set; }
        public string message { get; set; }
        public string keypress { get; set; }
        public string tries { get; set; }
        public string number { get; set; }
        public string callerid { get; set; }
        public string priority { get; set; }
        public string grouping { get; set; }
        public string name { get; set; }
        public string retries { get; set; }
        public string preamble { get; set; }
        public string postscript { get; set; }
        public string result { get; set; }
        }



    public class text
    {
        public string id { get; set; }
        public string delivery { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        public string grouping { get; set; }
        public string number { get; set; }
        public string result { get; set; }
        public string status { get; set; }
    }

    public class fax
    {
        public string id { get; set; }
        public string number { get; set; }
        //Base 64 encoded data of the file to be faxed.only PDF format is supported
        public string content { get; set; }
        public string grouping { get; set; }
        public string result { get; set; }
        public string status { get; set; }

    }

    [XmlRoot("email")]
    public class email
    {
        public string id { get; set; }
        public string delivery { get; set; }
        public string template { get; set; }
        public from from { get; set; }
        public to to { get; set; }
        public string subject { get; set; }
        [XmlElement("event")]
        public string event_ { get; set; }
        public string append { get; set; }

        public email()
        {
            this.from = new from();
            this.to = new to();
        }
    }


    public class from
    {
        public string name { get; set; }

    }

    public class to
    {
        public string name { get; set; }
        public string email { get; set; }
    }

}
