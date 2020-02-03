namespace NServiceBus.Gateway.RavenDB
{
    using Raven.Client;
    using Raven.Client.Documents.Session;
    using System;
    using System.Threading.Tasks;

    sealed class RavenDeduplicationSession : IDeduplicationSession
    {
        public bool IsDuplicate { get; }

        public async Task MarkAsDispatched()
        {
            if (!IsDuplicate)
            {
                var timeReceived = DateTime.UtcNow;
                var expiry = timeReceived + deduplicationDataTimeToLive;

                var gatewayMessage = new GatewayMessage()
                {
                    Id = MessageIdHelper.EscapeMessageId(messageId),
                    TimeReceived = timeReceived
                };

                await session.StoreAsync(gatewayMessage);
                session.Advanced.GetMetadataFor(gatewayMessage)[Constants.Documents.Metadata.Expires] = expiry;
                await session.SaveChangesAsync();
            }
        }

        public RavenDeduplicationSession(IAsyncDocumentSession session, bool isDuplicate, string messageId, TimeSpan deduplicationDataTimeToLive)
        {
            this.session = session;
            IsDuplicate = isDuplicate;
            this.messageId = messageId;
            this.deduplicationDataTimeToLive = deduplicationDataTimeToLive;
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
        readonly TimeSpan deduplicationDataTimeToLive;
    }
}
