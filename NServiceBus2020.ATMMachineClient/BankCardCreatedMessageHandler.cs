using NServiceBus;
using NServiceBus.Logging;
using NServiceBus2020.Domain.Events;
using System;
using System.Threading.Tasks;

namespace NServiceBus2020.ATMMachineClient
{
    public class BankCardCreatedMessageHandler : IHandleMessages<BankCardCreated>
    {
        static ILog log = LogManager.GetLogger<BankCardCreatedMessageHandler>();

        protected ICurrentCardNumber _currentCardNumber;

        public BankCardCreatedMessageHandler(ICurrentCardNumber currentCardNumber)
        {
            _currentCardNumber = currentCardNumber;
        }

        public Task Handle(BankCardCreated message, IMessageHandlerContext context)
        {
            var response = $"bank card {message.Id} has been sent out in the virtual post";
            log.Info(response);
            Console.WriteLine(response);
            _currentCardNumber.Number = message.Id;
            return Task.CompletedTask;
        }
    }
}
