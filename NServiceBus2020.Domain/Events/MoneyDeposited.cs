using System;
using NServiceBus2020.DomainBase;
using NServiceBus2020.DomainBase.Interfaces;

namespace NServiceBus2020.Domain.Events
{
    public class MoneyDeposited : DomainEvent, IDomainEvent
    {
        public string CardNumber => Id;

        public double Quantity { get; set; }

        // Need for deserialize 
        public MoneyDeposited() { }

        public MoneyDeposited(AuditInfo auditInfo, string id, double quantity, DateTime timeStamp)
            : base(auditInfo, id, timeStamp)
        {
            Quantity = quantity;
        }
    }
}
