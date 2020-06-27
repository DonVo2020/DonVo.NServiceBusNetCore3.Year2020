﻿using System.Collections.Generic;

namespace NServiceBus2020.DomainBase.Interfaces
{
    public interface IAggregate
    {
        int Version { get; }
        string AggregateId { get; }
        void ApplyEvent(IDomainEvent @event);

        IEnumerable<IDomainEvent> UncommitedEvents();
        void ClearUncommitedEvents();
    }
}
