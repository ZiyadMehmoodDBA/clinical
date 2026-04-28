using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.Model.Messages
{
    public class UserMessagesModel
    {

        public string PatientId { get; set; }
        public string hfMessageTo { get; set; }
        public string AttatchedPatientId { get; set; }
        public string Subject { get; set; }
        public string MessageDtl123 { get; set; }
        public string Priority { get; set; }
        public string Priority_text { get; set; }
        public string CommandType { get; set; }
        public string UserMessagesIds { get; set; }
        public string UserMesgId { get; set; }
        public string MessageName { get; set; }
        public string MessageDate { get; set; }
        public string PageNumber { get; set; }
        public string RowsPerPage { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public byte[] FileStream { get; set; }
        public dynamic Files { get; set; }
        public string PatientLetterId { get; set; }
        public string IsPatientMessage { get; set; }
        public string UniqueNumber { get; set; }
        public string MessageType { get; set; }
        public string MessageTo { get; set; }
        public string Username { get; set; }
        public string UsernameFrom { get; set; }
        public string SecretKey { get; set; }
        public string Password { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string DirectMsgId { get; set; }
        public string MessageStatus { get; set; }
        public DateTime DateTime { get; set; }
        public string MessageDetail { get; set; }
        public string msgType { get; set; }


    }
}
