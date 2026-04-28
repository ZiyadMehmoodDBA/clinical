using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Patient
{
  public class PatientDocumentModel
    {
        public string PatDocId { get; set; }
        public string DocumentType { get; set; }
        public string Documentid { get; set; }
        public string DocumentName { get; set; }
        public string PatientId { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public byte[] FileStream { get; set; }
        public string Url { get; set; }   
        public string Base64FileStream { get; set; }
        public string OrderId { get; set; }
        public string ExpiryDate { get; set; }

    }
    public class PatientDocumentResponse
    {
        public bool status { get; set; }
        public string Message { get; set; }
        public string PatDocId { get; set; }
    }

    public class DocumentPrivacyModel
    {
        public bool PatientId { get; set; }
        public string PatDocId { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public string ShowPasswordAlert { get; set; }
    }

}

