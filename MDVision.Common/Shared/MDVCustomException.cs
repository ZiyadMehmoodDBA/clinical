using System;

namespace MDVision.Common.Shared
{
    public class MDVCustomException
    {
        public static string HumanReadableMessage(string systemMessage)
        {
            string humanReadableMessage = "";
            systemMessage = systemMessage.ToLower();

            if (systemMessage.IndexOf("object reference", StringComparison.Ordinal) >= 0)
            {
                humanReadableMessage = "Object Not Found";
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