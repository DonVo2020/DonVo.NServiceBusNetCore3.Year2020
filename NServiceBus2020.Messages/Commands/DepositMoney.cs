using NServiceBus2020.DomainBase;
using NServiceBus2020.MessageBase.Interfaces;

namespace NServiceBus2020.Messages.Commands
{
    public class DepositMoney : BusMessage, ICommand
    {
        public string CardNumber { get; set; }
        public double Quantity { get; set; }

        public DepositMoney(AuditInfo auditInfo, string cardNumber, double quantity) : base(auditInfo)
        {
            CardNumber = cardNumber;
            Quantity = quantity;
        }
    }
}
