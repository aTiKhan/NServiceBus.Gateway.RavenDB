namespace NServiceBus.Gateway.RavenDB
{
    using Extensibility;
    using Raven.Client.Documents;
    using System;
    using System.Threading.Tasks;

    class RavenGatewayDeduplicationStorage : IGatewayDeduplicationStorage
    {
        public RavenGatewayDeduplicationStorage(IDocumentStore documentStore, TimeSpan deduplicationDataTimeToLive)
        {
            this.documentStore = documentStore;
            this.deduplicationDataTimeToLive = deduplicationDataTimeToLive;
        }

        public bool SupportsDistributedTransactions => false;

        public async Task<IDeduplicationSession> CheckForDuplicate(string messageId, ContextBag context)
        {
            var session = documentStore.OpenAsyncSession();
            var isDuplicate = await session.LoadAsync<GatewayMessage>(MessageIdHelper.EscapeMessageId(messageId)) != null;

            return new RavenDeduplicationSession(session, isDuplicate, messageId, deduplicationDataTimeToLive);
        }

        readonly IDocumentStore documentStore;
        readonly TimeSpan deduplicationDataTimeToLive;
    }
}
