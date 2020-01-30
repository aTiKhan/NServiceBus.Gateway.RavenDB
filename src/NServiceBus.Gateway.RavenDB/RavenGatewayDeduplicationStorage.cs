using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Settings;
using Raven.Client.Documents;
using System;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.RavenDB
{
    class RavenGatewayDeduplicationStorage : IGatewayDeduplicationStorage
    {
        public RavenGatewayDeduplicationStorage(Func<IBuilder, ReadOnlySettings, IDocumentStore> documentStoreFactory)
        {
            this.documentStoreFactory = documentStoreFactory;
        }

        public bool SupportsDistributedTransactions => false;

        public Task<IDeduplicationSession> CheckForDuplicate(string messageId, ContextBag context)
        {
            throw new NotImplementedException();
        }

        readonly Func<IBuilder, ReadOnlySettings, IDocumentStore> documentStoreFactory;
    }
}
