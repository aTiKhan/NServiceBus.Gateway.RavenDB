namespace NServiceBus.Gateway.RavenDB
{
    using System;

    class GatewayMessage
    {
        public string Id { get; set; }

        public DateTime TimeReceived { get; set; }
    }
}
