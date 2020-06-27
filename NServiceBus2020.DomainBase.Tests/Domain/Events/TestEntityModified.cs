using NServiceBus2020.DomainBase.Interfaces;

namespace NServiceBus2020.DomainBase.Tests.Domain.Events
{

    public class TestEntityModified : DomainEvent, IDomainEvent
    {
        public string TestProperty { get; set; }

        // Need for deserialize 
        public TestEntityModified() { }

        public TestEntityModified(string id, AuditInfo auditInfo, string testProperty) : base(auditInfo, id)
        {
            TestProperty = testProperty;
        }
    }
}
