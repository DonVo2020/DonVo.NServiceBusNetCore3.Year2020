using NServiceBus2020.DomainBase;
using NServiceBus2020.DomainBase.Interfaces;
using System;

namespace NServiceBus2020.Domain.Events
{
    public class BankCardCreated : DomainEvent, IDomainEvent
    {
        public string ClientId { get; set; }
        // Need for deserialize 
        public BankCardCreated() { }

        public BankCardCreated(AuditInfo auditInfo, string id, string clientId, DateTime? timeStamp = null)
            : base(auditInfo, id, timeStamp)
        {
            ClientId = clientId;
        }
    }
}
