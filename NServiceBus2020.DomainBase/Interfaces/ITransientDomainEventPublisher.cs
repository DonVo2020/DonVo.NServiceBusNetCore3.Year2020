using System.Threading.Tasks;

namespace NServiceBus2020.DomainBase.Interfaces
{
    public interface ITransientDomainEventPublisher
    {
        Task PublishAsync<T>(T publishedEvent);
    }

}
