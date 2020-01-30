using NServiceBus.Extensibility;
using Raven.Client.Documents;
using System;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.RavenDB
{
    class RavenDeduplicationSession : IDeduplicationSession
    {
        public bool IsDuplicate
        {
            get
            {
                using (var session = documentStore.OpenSession())
                {
                    return session.Load<GatewayMessage>(EscapeMessageId(messageId)) != null;
                }
            }
        }

        public async Task MarkAsDispatched()
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                var gatewayMessage = await session.LoadAsync<GatewayMessage>(EscapeMessageId(messageId));
                if (gatewayMessage == null)
                {
                    await session.StoreAsync(new GatewayMessage()
                    {
                        Id = EscapeMessageId(messageId),
                        TimeReceived = DateTime.UtcNow
                    });
                }
            }
        }

        public RavenDeduplicationSession(IDocumentStore documentStore, string messageId, ContextBag context)
        {
            this.documentStore = documentStore;
            this.messageId = messageId;
            this.context = context;
        }

        public void Dispose()
        {

        }

        static string EscapeMessageId(string messageId)
        {
            return messageId.Replace("\\", "_");
        }

        readonly IDocumentStore documentStore;
        readonly string messageId;
        readonly ContextBag context;
    }
}
