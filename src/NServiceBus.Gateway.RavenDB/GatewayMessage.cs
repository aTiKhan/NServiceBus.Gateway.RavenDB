using System;

namespace NServiceBus.Gateway.RavenDB
{
    class GatewayMessage
    {
        public string Id { get; set; }

        public DateTime TimeReceived { get; set; }
    }
}
