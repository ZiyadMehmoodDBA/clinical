using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Clinical.LegacyNotes
{
    public class NoteHeaderData
    {

        public string Type { get; set; }
        public DateTime? Notedate { get; set; }
        public string NoteReason { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string EmailAddress { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PracticeId { get; set; }
        public string ShortName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIPCode { get; set; }
        public Int64 ProviderId { get; set; }
        public string MiddleInitial { get; set; }
        public string SpecialtyName { get; set; }
        public byte[] eSignature { get; set; }

    }

    public class Provider
    {
        public Int64 ProviderId { get; set; }
        public string FirstName { get; set; }
        public string ShortName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string OfficeAddress { get; set; }
        public byte[] eSignature { get; set; }
    }

}
