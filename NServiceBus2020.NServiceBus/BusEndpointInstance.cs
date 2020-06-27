using System.Threading.Tasks;
using NServiceBus;
using StructureMap;

namespace NServiceBus2020.NServiceBus
{
    public static class BusEndpointInstance
    {
        [System.Obsolete]
        public static async Task<IEndpointInstance> Learning(string endpoint, IContainer container =null)
        {
            EndpointConfiguration endpointConfiguration = LearningEndpointConfiguration(endpoint,container);
            ConventionExtensions.ApplyCustomConventions(endpointConfiguration);
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            return endpointInstance;
        }

        [System.Obsolete]
        public static EndpointConfiguration LearningEndpointConfiguration(string endpoint, IContainer container=null)
        {
            var endpointConfiguration = new EndpointConfiguration(endpoint);
            if (container != null)
            {
                endpointConfiguration.UseContainer<StructureMapBuilder>(
                    customizations: customizations => { customizations.ExistingContainer(container); });
            }
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();
            return endpointConfiguration;
        }
    }
}
