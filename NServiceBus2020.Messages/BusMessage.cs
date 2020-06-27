using NServiceBus2020.DomainBase;

namespace NServiceBus2020.Messages
{
    public class BusMessage : MessageBase.BusMessage
    {
        public AuditInfo AuditInfo { get; set; }

        public BusMessage(AuditInfo auditInfo) : base()
        {
            AuditInfo = auditInfo;
        }
    }
}