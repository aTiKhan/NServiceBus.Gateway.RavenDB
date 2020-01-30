using NServiceBus.ObjectBuilder;
using NServiceBus.Settings;

namespace NServiceBus.Gateway.RavenDB
{
    /// <summary>
    /// Configures the deduplication storage.
    /// </summary>
    public class RavenGatewayDeduplicationConfiguration : GatewayDeduplicationConfiguration
    {
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
            return new RavenGatewayDeduplicationStorage();
        }
    }
}
