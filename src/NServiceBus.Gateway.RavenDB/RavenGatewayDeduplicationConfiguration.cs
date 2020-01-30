using NServiceBus.ObjectBuilder;
using NServiceBus.Settings;
using Raven.Client.Documents;
using System;

namespace NServiceBus.Gateway.RavenDB
{
    /// <summary>
    /// Configures the deduplication storage.
    /// </summary>
    public class RavenGatewayDeduplicationConfiguration : GatewayDeduplicationConfiguration
    {
        /// <summary>
        /// Initialize a new instance of the RavenDB gateway deduplication configuration
        /// </summary>
        public RavenGatewayDeduplicationConfiguration(Func<IBuilder, ReadOnlySettings, IDocumentStore> documentStoreFactory)
        {
            Guard.AgainstNull(nameof(documentStoreFactory), documentStoreFactory);

            this.documentStoreFactory = documentStoreFactory;
        }

        /// <summary>
        /// Invoked when the endpoint configuration completed to initialize the storage or
        ///  verify configuration before the endpoint starts.
        /// </summary>
        public override void Setup(ReadOnlySettings settings)
        {
            base.Setup(settings);
        }

        /// <summary>
        /// Creates an instance of the deduplication storage.
        /// </summary>
        public override IGatewayDeduplicationStorage CreateStorage(IBuilder builder)
        {
            return new RavenGatewayDeduplicationStorage(documentStoreFactory);
        }

        readonly Func<IBuilder, ReadOnlySettings, IDocumentStore> documentStoreFactory;
    }
}
