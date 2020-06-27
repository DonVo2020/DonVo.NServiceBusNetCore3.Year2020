using NServiceBus2020.DomainBase;
using NServiceBus2020.MessageBase.Interfaces;

namespace NServiceBus2020.Messages.Commands
{
    public class RequestBankCard: BusMessage, ICommand
    {

        public string CardNumber { get; set; }
        public string ClientId { get; set; }

        public RequestBankCard(AuditInfo auditInfo, string cardNumber, string clientId) : base(auditInfo)
        {
            CardNumber = cardNumber;
            ClientId = clientId;
        }
    }
}
