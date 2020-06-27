using System;

namespace NServiceBus2020.DomainBase.Interfaces
{
    public interface IDomainEvent
    {
        string Id { get; }

        DateTime TimeStamp { get; set; }

        AuditInfo AuditInfo { get; set; }

    }
}
