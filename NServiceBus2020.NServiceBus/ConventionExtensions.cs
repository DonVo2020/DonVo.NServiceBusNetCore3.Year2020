using NServiceBus;

namespace NServiceBus2020.NServiceBus
{
    public static class ConventionExtensions
    {
        public static void ApplyCustomConventions(this EndpointConfiguration endpointConfiguration)
        {
            ConventionsBuilder conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(
                type =>
                {
                    return type.Namespace != null && type.Namespace.EndsWith(CustomConventionsEnum.Commands.ToString());
                });
            conventions.DefiningEventsAs(
                type =>
                {
                    return type.Namespace != null && (type.Namespace.EndsWith(CustomConventionsEnum.Events.ToString()));
                });
            conventions.DefiningMessagesAs(
                type =>
                {
                    return type.Namespace != null && type.Namespace.EndsWith(CustomConventionsEnum.Messages.ToString());
                });
        }
    }
}