using NServiceBus.Extensibility;
using System;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.RavenDB
{
    class RavenGatewayDeduplicationStorage : IGatewayDeduplicationStorage
    {
        public bool SupportsDistributedTransactions => false;

        public Task<IDeduplicationSession> CheckForDuplicate(string messageId, ContextBag context)
        {
            throw new NotImplementedException();
        }
    }
}
