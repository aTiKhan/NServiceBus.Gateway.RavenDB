using Raven.Client.Documents.Session;
using System;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.RavenDB
{
    sealed class RavenDeduplicationSession : IDeduplicationSession
    {
        public bool IsDuplicate { get; }

        public async Task MarkAsDispatched()
        {
            if (!IsDuplicate)
            {
                await session.StoreAsync(new GatewayMessage()
                {
                    Id = MessageIdHelper.EscapeMessageId(messageId),
                    TimeReceived = DateTime.UtcNow
                });
                await session.SaveChangesAsync();
            }
        }

        public RavenDeduplicationSession(IAsyncDocumentSession session, bool isDuplicate, string messageId)
        {
            this.session = session;
            IsDuplicate = isDuplicate;
            this.messageId = messageId;
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    session?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        bool disposedValue = false;
        readonly IAsyncDocumentSession session;
        readonly string messageId;
    }
}
