using NServiceBus2020.Domain.Events;
using NServiceBus2020.Domain.Exception;
using NServiceBus2020.DomainBase;
using NServiceBus2020.DomainBase.Interfaces;
using System;

namespace NServiceBus2020.Domain.Aggregate
{
    public class BankCard: AggregateRoot, IAggregate
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public double Balance { get; set; }
        public DateTime Created { get; private set; }
        public override string AggregateId => Id;

        public BankCard()
        {
            RegisterTransition<BankCardCreated>(Apply);
            RegisterTransition<MoneyDeposited>(Apply);
            RegisterTransition<MoneyWithdrawn>(Apply);
        }

        private void Apply(BankCardCreated obj)
        {
            Id = obj.Id;
            ClientId = obj.ClientId;
            Balance = 0;
            Created = DateTime.UtcNow;
        }

        private void Apply(MoneyDeposited obj)
        {
            Balance += obj.Quantity;
        }

        private void Apply(MoneyWithdrawn obj)
        {
            Balance -= obj.Quantity;
        }

        public BankCard(AuditInfo auditInfo, string id, string clientId, DateTime timeStamp) : this()
        {
            RaiseEvent(new BankCardCreated(auditInfo, id, clientId, timeStamp));
        }

        public static BankCard RequestBankCard(AuditInfo auditInfo, string id, string clientId, DateTime timeStamp)
        {
            return new BankCard(auditInfo, id, clientId, timeStamp);
        }

        public void Deposit(AuditInfo auditInfo, double quantity , DateTime timeStamp) 
        {
            RaiseEvent(new MoneyDeposited(auditInfo, Id, quantity, timeStamp));
        }
        public void Withdraw(AuditInfo auditInfo, double quantity, DateTime timeStamp)
        {
            if (quantity > Balance)
                throw new WithDrawException($"Can not withdraw {quantity} as insufficent funds. Balance on the card is {Balance}.");
            RaiseEvent(new MoneyWithdrawn(auditInfo, Id, quantity, timeStamp));
        }
    }
}
