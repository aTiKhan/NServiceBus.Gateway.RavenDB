using NServiceBus.Extensibility;
using Raven.Client.Documents;
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

        public async Task<IDeduplicationSession> CheckForDuplicate(string messageId, ContextBag context)
        {
            var session = documentStore.OpenAsyncSession();
            var isDuplicate = await session.LoadAsync<GatewayMessage>(MessageIdHelper.EscapeMessageId(messageId)) != null;

            return new RavenDeduplicationSession(session, isDuplicate, messageId);
        }

        readonly IDocumentStore documentStore;
    }
}
