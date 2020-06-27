using NServiceBus2020.DomainBase.Interfaces;

namespace NServiceBus2020.DomainBase.Tests.Domain.Events
{

    public class TestEntityCreated : DomainEvent, IDomainEvent
    {
        public string TestProperty { get; set; }

        // Need for deserialize 
        public TestEntityCreated() { }

        public TestEntityCreated(string id, AuditInfo auditInfo) : base(auditInfo, id)
        {
        }
    }
}
