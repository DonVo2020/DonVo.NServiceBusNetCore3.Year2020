using NServiceBus;
using NServiceBus.Logging;
using NServiceBus2020.DomainBase.Exception;
using NServiceBus2020.DomainBase.Interfaces;
using NServiceBus2020.Messages.Commands;
using System;
using System.Threading.Tasks;

namespace NServiceBus2020.BankTransactionsService
{
    public class DepositMoneyMessageHandler : IHandleMessages<DepositMoney>
    {
        protected IDomainRepository _domainRepository;

        static ILog log = LogManager.GetLogger<DepositMoneyMessageHandler>();

        public DepositMoneyMessageHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public Task Handle(DepositMoney message, IMessageHandlerContext context)
        {

            if (!_domainRepository.Exists<Domain.Aggregate.BankCard>(message.CardNumber))
                throw new AggregateNotFoundException($"Bank card {message.CardNumber} was not found");
            var BankCard = _domainRepository.GetById<Domain.Aggregate.BankCard>(message.CardNumber);
            BankCard.Deposit(message.AuditInfo,message.Quantity,DateTime.UtcNow);
            _domainRepository.Save(BankCard);
            log.Info($"card {BankCard.Id} balance is now {BankCard.Balance}");
            return Task.CompletedTask;
        }
    }
}
