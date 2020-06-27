using NServiceBus;
using NServiceBus.Logging;
using NServiceBus2020.Messages.Messages;
using System;
using System.Threading.Tasks;

namespace NServiceBus2020.ATMMachineClient
{
    public class WithDrawReponseMessageHandler : IHandleMessages<WithDrawReponse>
    {
        static ILog log = LogManager.GetLogger<WithDrawReponseMessageHandler>();


        public WithDrawReponseMessageHandler()
        {
        }

        public Task Handle(WithDrawReponse message, IMessageHandlerContext context)
        {
            Console.WriteLine(message.WithDrawValid
                ? $"{message.Quantity} has been virtual dispensed. Please collect the money"
                : message.Message);

            return Task.CompletedTask;
        }
    }
}
