using NServiceBus;
using NServiceBus2020.DomainBase.Interfaces;
using NServiceBus2020.Messages.Commands;
using System;
using System.Threading.Tasks;

namespace NServiceBus2020.BankTransactionsService
{
    public class RequestBankCardMessageHandler :
        IHandleMessages<RequestBankCard>
    {
        protected IDomainRepository _domainRepository;

        public RequestBankCardMessageHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public Task Handle(RequestBankCard message, IMessageHandlerContext context)
        {;
            var BankCard = Domain.Aggregate.BankCard.RequestBankCard(message.AuditInfo, message.CardNumber, message.ClientId, DateTime.UtcNow);
            _domainRepository.Save(BankCard);
            return Task.CompletedTask;
        }
    }
}
