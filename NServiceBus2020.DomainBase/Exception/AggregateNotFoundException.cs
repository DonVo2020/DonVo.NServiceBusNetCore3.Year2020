
namespace NServiceBus2020.DomainBase.Exception
{
    public class AggregateNotFoundException : System.Exception
    {
        public AggregateNotFoundException(string message) : base(message)
        {
        }
    }
}