using MDVision.Business.AppointmentReminders.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MDVision.Business.AppointmentReminders.Response
{
    public class response
    {
        public status status { get; set; }
        public errors errors { get; set; }
        public response()
        {
            this.status = new status();
            this.errors = new errors();
        }
    }


    [XmlRoot("status")]
    public class status
    {
        [XmlElement("call")]
        public List<Response_call> call { get; set; }
        [XmlElement("text")]
        public List<Response_text> text { get; set; }
        [XmlElement("fax")]
        public List<fax> fax { get; set; }
        [XmlElement("email")]
        public List<Response_email> email { get; set; }
        public status()
        {
            this.call = new List<Response_call>();
            this.text = new List<Response_text>();
            this.fax = new List<fax>();
            this.email = new List<Response_email>();
        }
    }

    [XmlRoot("errors")]
    public class errors
    {
        [XmlElement("error")]
        public List<error> error { get; set; }
        public errors()
        {
            this.error = new List<error>();
        }
    }

    public class error
    {
        public string message { get; set; }
        public string line { get; set; }
    }

    [XmlRoot("call")]
    public class Response_call
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
        public error error { get; set; }

        public Response_call()
        {
            this.error = new error();
        }

    }

    [XmlRoot("text")]
    public class Response_text
    {
        public string id { get; set; }
        public string delivery { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        public string grouping { get; set; }
        public string number { get; set; }
        public string result { get; set; }
        public string status { get; set; }
        public error error { get; set; }
        public reply reply { get; set; }

        public Response_text()
        {
            this.error = new error();
            this.reply = new reply();
        }
    }

    [XmlRoot("email")]
    public class Response_email
    {
        public string id { get; set; }
        [XmlElement("action")]
        public List<Email_action> action { get; set; }
        public string message { get; set; }
        public string notice { get; set; }
        public string result { get; set; }
        public error error { get; set; }

        public Response_email()
        {
            this.action = new List<Email_action>();
        }
    }

    [XmlRoot("action")]
    public class Email_action
    {
        [XmlText]
        public string Value { get; set; }
        [XmlAttribute("type")]
        public string type { get; set; }
    }

    [XmlRoot("reply")]
    public class reply
    {
        public string message { get; set; }
        public string receipt { get; set; }
    }


}
