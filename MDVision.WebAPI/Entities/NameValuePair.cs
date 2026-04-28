using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.WebAPI.Entities
{
    public class NameValuePair
    {
        private string _name;
        private string _value;
        private string _Refvalue;
        private string _Refname;
        private string _IsActive;
        private string _ExName;
        private string _ExValue;

        public string Name { get { return _name; } set { _name = value; } }
        public string Value { get { return _value; } set { _value = value; } }
        public string RefValue { get { return _Refvalue; } set { _Refvalue = value; } }
        public string RefName { get { return _Refname; } set { _Refname = value; } }
        public string IsActive { get { return _IsActive; } set { _IsActive = value; } }
        public string ExName { get { return _ExName; } set { _ExName = value; } }
        public string ExValue { get { return _ExValue; } set { _ExValue = value; } }

        public NameValuePair() { }

        public NameValuePair(string name, string value, string Refvalue = "", string Refname = "", string IsActive = "", string Exvalue = "", string Exname = "")
        {
            this.Name = name;
            this.Value = value;
            this.RefValue = Refvalue;
            this.RefName = Refname;
            this.IsActive = IsActive;
            this.ExName = Exname;
            this.ExValue = Exvalue;
        }

    }
}