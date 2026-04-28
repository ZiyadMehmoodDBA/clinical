using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MDVision.Business.AppointmentReminders
{
    public static class ReminderConfiguration
    {
        private static int Confirm_ = 1;
        private static int Cancel_ = 2;
        private static string CallConfirmMessage_ = " Press " + Confirm + " to confirm.";
        private static string CallCancelMessage_ = " Press " + Cancel + " to cancel.";
        private static string TextConfirmMessage_ = " Reply with " + Confirm + " to confirm.";
        private static string TextCancelMessage_ = " Reply with " + Cancel + " to cancel.";
        private static string URL_ = ConfigurationManager.AppSettings["ReminderCallURL"];

        public static int Confirm
        {
            get { return Confirm_; }
        }
        public static int Cancel
        {
            get { return Cancel_; }
        }
        public static string CallConfirmMessage
        {
            get { return CallConfirmMessage_; }
        }
        public static string CallCancelMessage
        {
            get { return CallCancelMessage_; }
        }
        public static string TextConfirmMessage
        {
            get { return TextConfirmMessage_; }
        }
        public static string TextCancelMessage
        {
            get { return TextCancelMessage_; }
        }
        public static string URL
        {
            get { return URL_; }
        }
    }
}
