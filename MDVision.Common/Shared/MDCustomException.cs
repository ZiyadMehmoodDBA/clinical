using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Common.Shared
{
    public class MDCustomException : Exception
    {
        public string ExtraErrorInfo { get; set; }

        public MDCustomException() : base()
        {
        }

        public MDCustomException(string message) : base(message)
        {
        }

        public MDCustomException(string message, Exception e) : base(message, e)
        {
        }

        public static string HumanReadableMessage(string systemMessage)
        {
            string humanReadableMessage = "";
            systemMessage = systemMessage.ToLower();

            if (systemMessage.IndexOf("object reference", StringComparison.Ordinal) >= 0)
            {
                humanReadableMessage = "Could Not Load Data";
            }
            else if (systemMessage.IndexOf("some other error", StringComparison.Ordinal) >= 0)
            {
                humanReadableMessage = "custom message";
            }
            else
            {
                humanReadableMessage = systemMessage;
            }
            return humanReadableMessage;
        }
    }
}
