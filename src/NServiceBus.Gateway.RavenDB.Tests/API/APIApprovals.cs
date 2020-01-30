namespace NServiceBus.Gateway.RavenDB.Tests.API
{
    using NUnit.Framework;
    using Particular.Approvals;
    using PublicApiGenerator;

    [TestFixture]
    class APIApprovals
    {
        [Test]
        public void ApproveNServiceBusGatewayRavenDB()
        {

            var publicApi = ApiGenerator.GeneratePublicApi(typeof(RavenGatewayDeduplicationConfiguration).Assembly, excludeAttributes: new[] { "System.Runtime.Versioning.TargetFrameworkAttribute" });
            Approver.Verify(publicApi);
        }
    }
}
