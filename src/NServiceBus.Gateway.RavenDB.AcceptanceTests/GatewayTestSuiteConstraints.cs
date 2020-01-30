using NServiceBus.AcceptanceTesting.Support;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.Gateway.RavenDB;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.AcceptanceTests
{
    public partial class GatewayTestSuiteConstraints : IGatewayTestSuiteConstraints
    {
        public Task ConfigureDeduplicationStorage(string endpointName, EndpointConfiguration configuration, RunSettings settings)
        {
            var ravenGatewayDeduplicationConfiguration = new RavenGatewayDeduplicationConfiguration((builder, _)=> 
            {
                return null;
            });
            var gatewaySettings = configuration.Gateway(ravenGatewayDeduplicationConfiguration);
            configuration.GetSettings().Set(gatewaySettings);

            return Task.FromResult(false);
        }

        public Task Cleanup()
        {
            //TODO: clean up db once tests are completed

            return Task.FromResult(false);
        }
    }
}
