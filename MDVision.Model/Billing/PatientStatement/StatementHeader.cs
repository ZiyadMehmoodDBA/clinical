using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Billing.PatientStatement
{
    public class StatementHeader
    {

        public string AccountNumber { get; set; }
        public Int64 ChargeCapId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ZipCodeExt { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityState { get; set; }
        public string FacilityZipCode { get; set; }
        public string FacilityZipCodeExt { get; set; }
        public Int64 PatientId { get; set; }
        public Int64 FacilityId { get; set; }
        public Int64 LetterId { get; set; }
        public string LetterName { get; set; }
        public string LetterHtmlDocument { get; set; }
        public string FacilityDescription { get; set; }
        public string Description { get; set; }
        public string Procedure { get; set; }
        public string Message { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string FromCity { get; set; }
        public string FromState { get; set; }
        public string FromZip { get; set; }
        public string FromZipExt { get; set; }
        public string RemitToName { get; set; }
        public string RemitToAddress { get; set; }
        public string RemitToCity { get; set; }
        public string RemitToState { get; set; }
        public string RemitToZip { get; set; }
        public string RemitToZipExt { get; set; }
        public string OfcHoursFrom { get; set; }
        public string OfcHoursTo { get; set; }
        public string PhoneNo { get; set; }
        public string FullName { get; set; }
        public float Charges { get; set; }
        public float PatBalance { get; set; }
        public int Age { get; set; }
        public decimal InsBalance { get; set; }
        public int FirstMsgId { get; set; }
        public string FirstMessage { get; set; }
        public int SecondMsgId { get; set; }
        public string SecondMessage { get; set; }
        public int ThirdMsgId { get; set; }
        public string ThirdMessage { get; set; }
        public int FourthMsgId { get; set; }
        public string FourthMessage { get; set; }
        public int FifthMsgId { get; set; }
        public string FifthMessage { get; set; }

        public DateTime Date { get; set; }
    }
}
