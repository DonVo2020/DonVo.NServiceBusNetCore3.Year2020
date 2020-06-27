using NServiceBus2020.DomainBase;
using NServiceBus2020.DomainBase.Interfaces;
using System;

namespace NServiceBus2020.Domain.Events
{
    public class MoneyWithdrawn : DomainEvent, IDomainEvent
    {
        public string CardNumber => Id;

        public double Quantity { get; set; }

        // Need for deserialize 
        public MoneyWithdrawn() { }

        public MoneyWithdrawn(AuditInfo auditInfo, string id, double quantity, DateTime timeStamp)
            : base(auditInfo, id, timeStamp)
        {
            Quantity = quantity;
        }
    }
}
