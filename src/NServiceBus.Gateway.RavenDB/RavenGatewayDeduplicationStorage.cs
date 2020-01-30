using NServiceBus.Extensibility;
using Raven.Client.Documents;
using System;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.RavenDB
{
    class RavenGatewayDeduplicationStorage : IGatewayDeduplicationStorage
    {
        public RavenGatewayDeduplicationStorage(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public bool SupportsDistributedTransactions => false;

        public Task<IDeduplicationSession> CheckForDuplicate(string messageId, ContextBag context)
        {
            return Task.FromResult<IDeduplicationSession>(new RavenDeduplicationSession(documentStore, messageId, context));
        }

        readonly IDocumentStore documentStore;
    }
}
