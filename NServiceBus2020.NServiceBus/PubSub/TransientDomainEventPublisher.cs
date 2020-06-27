using NServiceBus;
using NServiceBus2020.DomainBase.Interfaces;
using System.Threading.Tasks;

namespace NServiceBus2020.NServiceBus.PubSub
{
    public class TransientDomainEventPublisher : ITransientDomainEventPublisher
    {
        private readonly IEndpointInstance _endpoint;

        public TransientDomainEventPublisher(IEndpointInstance endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task PublishAsync<T>(T publishedEvent)
        {
            await _endpoint.Publish(publishedEvent);
        }

        public async Task PublishAsync(object publishedEvent)
        {
            await _endpoint.Publish(publishedEvent);
        }
    }
}
