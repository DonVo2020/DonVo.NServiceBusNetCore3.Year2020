using NServiceBus;
using NServiceBus.Logging;
using NServiceBus2020.Domain.Exception;
using NServiceBus2020.DomainBase.Exception;
using NServiceBus2020.DomainBase.Interfaces;
using NServiceBus2020.Messages.Commands;
using NServiceBus2020.Messages.Messages;
using System;
using System.Threading.Tasks;

namespace NServiceBus2020.BankTransactionsService
{
    public class WithdrawMoneyMessageHandler: IHandleMessages<WithdrawMoney>
    {
        protected IDomainRepository _domainRepository;
        static ILog log = LogManager.GetLogger<DepositMoneyMessageHandler>();

        public WithdrawMoneyMessageHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public Task Handle(WithdrawMoney message, IMessageHandlerContext context)
        {
            var replyMessage = new WithDrawReponse(message.AuditInfo, message.CardNumber, message.Quantity, true); ;
            try
            {
                if (!_domainRepository.Exists<Domain.Aggregate.BankCard>(message.CardNumber))
                    throw new AggregateNotFoundException($"Bank card {message.CardNumber} was not found");
                var BankCard = _domainRepository.GetById<Domain.Aggregate.BankCard>(message.CardNumber);
                BankCard.Withdraw(message.AuditInfo, message.Quantity, DateTime.UtcNow);
                _domainRepository.Save(BankCard);
                log.Info($"card {BankCard.Id} balance is now {BankCard.Balance}");
                return context.Reply(replyMessage);
            }
            catch (WithDrawException ex)
            {
                replyMessage.Message = ex.Message;
                replyMessage.WithDrawValid = false;
                return context.Reply(replyMessage);
            }
     
        }
    }
}
