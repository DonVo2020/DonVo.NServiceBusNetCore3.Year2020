using Machine.Specifications;
using Moq;
using NServiceBus2020.BankTransactionsService;
using NServiceBus2020.Messages.Commands;
using It = Machine.Specifications.It;

namespace NServiceBus2020.BankTransactions.UnitTests.Scenarios.Application
{
    [Subject(typeof(RequestBankCard), "Command Handler")]
    public class when_handling_request_bank_card_command: HandlerSpec
    {
        protected static RequestBankCard RequestBankCardCommand;
        protected static RequestBankCardMessageHandler RequestBankCardMessageHandler;

        private Establish context = () =>
        {
            RequestBankCardMessageHandler = new RequestBankCardMessageHandler(Repository.Object);
            RequestBankCardCommand = new RequestBankCard(AuditInfo, CardNumber, "1");
        };
        Because of = () => RequestBankCardMessageHandler.Handle(RequestBankCardCommand, Context);
        It should_have_saved_bank_card = () => Repository.Verify(foo => foo.Save(Moq.It.IsAny<Domain.Aggregate.BankCard>(), false), Times.Exactly(1));
        private It should_have_saved_bank_card_with_no_balance = () => SavedBankCard.Balance.ShouldEqual(0);
    }
}
