namespace NServiceBus2020.ATMMachineClient
{
    public interface ICurrentCardNumber
    {
        string Number { get; set; }
    }

    public class CurrentCardNumber : ICurrentCardNumber
    {
        public string Number { get; set; } 
    }
}
