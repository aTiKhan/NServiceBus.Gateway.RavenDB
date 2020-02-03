using NServiceBus.ObjectBuilder;
using NServiceBus.Settings;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.ServerWide.Commands;
using System;
using System.Threading.Tasks;

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
            this.settings = settings;

            //TODO: kick off the clean-up task (as a FeatureStartupTask)

            base.Setup(settings);
        }

        /// <summary>
        /// Creates an instance of the deduplication storage.
        /// </summary>
        public override IGatewayDeduplicationStorage CreateStorage(IBuilder builder)
        {
            var documentStore = documentStoreFactory(builder, settings);

            EnsureClusterConfiguration(documentStore);
            EnableExpirationFeature(documentStore);

            return new RavenGatewayDeduplicationStorage(documentStore, DeduplicationDataTimeToLive);
        }

        static void EnableExpirationFeature(IDocumentStore documentStore)
        {
            documentStore.Maintenance.Send(new ConfigureExpirationOperation(new ExpirationConfiguration
            {
                Disabled = false,
                DeleteFrequencyInSec = 600
            }));
        }

        static void EnsureClusterConfiguration(IDocumentStore store)
        {
            using (var s = store.OpenSession())
            {
                var getTopologyCmd = new GetClusterTopologyCommand();
                s.Advanced.RequestExecutor.Execute(getTopologyCmd, s.Advanced.Context);

                var topology = getTopologyCmd.Result.Topology;

                // Currently do not support clusters with more than one possible primary member. Watchers (passive replication targets) are OK.
                if (topology.Members.Count != 1)
                {
                    throw new InvalidOperationException("RavenDB Persistence does not support RavenDB clusters with more than one Leader/Member node. Only clusters with a single Leader and (optionally) Watcher nodes are supported.");
                }
            }
        }

        /// <summary>
        /// The time to keep deduplication information, default value is 7 days
        /// </summary>
        public TimeSpan DeduplicationDataTimeToLive { get; set; } = TimeSpan.FromDays(7);

        ReadOnlySettings settings;
        readonly Func<IBuilder, ReadOnlySettings, IDocumentStore> documentStoreFactory;
    }
}
