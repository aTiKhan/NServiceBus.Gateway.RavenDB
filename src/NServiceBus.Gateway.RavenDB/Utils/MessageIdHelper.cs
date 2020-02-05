namespace NServiceBus.Gateway.RavenDB
{
    static class MessageIdHelper
    {
        public static string EscapeMessageId(string messageId)
        {
            return messageId.Replace("\\", "_");
        }
    }
}
