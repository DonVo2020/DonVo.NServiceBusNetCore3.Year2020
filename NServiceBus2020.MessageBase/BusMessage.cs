using System;

namespace NServiceBus2020.MessageBase
{
    public class BusMessage
    {
        public Guid TransactionId { get; set; }
 
        public BusMessage()
        {
            TransactionId = Guid.NewGuid();
        }
    }
}
