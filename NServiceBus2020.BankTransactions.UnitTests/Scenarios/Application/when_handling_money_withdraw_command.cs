using Machine.Specifications;
using Moq;
using NServiceBus2020.BankTransactionsService;
using NServiceBus2020.DomainBase.Exception;
using NServiceBus2020.Messages.Commands;
using NServiceBus2020.Messages.Messages;
using System;
using It = Machine.Specifications.It;

namespace NServiceBus2020.BankTransactions.UnitTests.Scenarios.Application
{
    [Subject(typeof(WithdrawMoney), "Command Handler")]
    public class when_handling_money_withdraw_command : HandlerSpec
    {
        protected static WithdrawMoney WithdrawMoneyCommand;
        protected static WithdrawMoneyMessageHandler WithdrawMoneyMessageHandler;

        private Establish context = () =>
        {
            WithdrawMoneyMessageHandler = new WithdrawMoneyMessageHandler(Repository.Object);
            WithdrawMoneyCommand = new WithdrawMoney(AuditInfo, CardNumber, 100);
            Repository.Setup(c => c.GetById<Domain.Aggregate.BankCard>(CardNumber)).Returns(new Domain.Aggregate.BankCard { Id = CardNumber, ClientId = "1", Balance = 200});
            Repository.Setup(c => c.Exists<Domain.Aggregate.BankCard>(CardNumber)).Returns(true);
        };
        Because of = () => WithdrawMoneyMessageHandler.Handle(WithdrawMoneyCommand, Context);
        It should_have_saved_bank_card = () => Repository.Verify(foo => foo.Save(Moq.It.IsAny<Domain.Aggregate.BankCard>(), false), Times.Exactly(1));
        It should_have_saved_bank_card_with_balance = () => SavedBankCard.Balance.ShouldEqual(100);
        It should_send_valid_withdraw_reponse = () => ((WithDrawReponse)Context.RepliedMessages[0].Message).WithDrawValid.ShouldBeTrue();
    }

    public class when_handling_money_withdraw_command_that_has_zero_balance : HandlerSpec
    {
        protected static WithdrawMoney WithdrawMoneyCommand;
        protected static WithdrawMoneyMessageHandler WithdrawMoneyMessageHandler;

        private readonly Establish context = () =>
        {
            WithdrawMoneyMessageHandler = new WithdrawMoneyMessageHandler(Repository.Object);
            WithdrawMoneyCommand = new WithdrawMoney(AuditInfo, CardNumber, 100);
            Repository.Setup(c => c.GetById<Domain.Aggregate.BankCard>(CardNumber)).Returns(new Domain.Aggregate.BankCard { Id = CardNumber, ClientId = "1", Balance = 0 });
            Repository.Setup(c => c.Exists<Domain.Aggregate.BankCard>(CardNumber)).Returns(true);
        };
        Because of = () => WithdrawMoneyMessageHandler.Handle(WithdrawMoneyCommand, Context);
        It should_have_not_saved_bank_card_changes = () => Repository.Verify(foo => foo.Save(Moq.It.IsAny<Domain.Aggregate.BankCard>(), false), Times.Exactly(0));
        It should_send_validwith_draw_reponse = () => ((WithDrawReponse)Context.RepliedMessages[0].Message).WithDrawValid.ShouldBeFalse();
        It should_send_validwith_draw_reponse_with_the_reason = () => ((WithDrawReponse)Context.RepliedMessages[0].Message).Message.ShouldEqual("Can not withdraw 100 as insufficent funds. Balance on the card is 0.");

    }


    [Subject(typeof(WithdrawMoney), "Command Handler")]
    public class when_handling_money_withdraw_command_where_no_card_exists : HandlerSpec
    {
        protected static WithdrawMoney WithdrawMoneyCommand;
        protected static WithdrawMoneyMessageHandler WithdrawMoneyMessageHandler;
        protected static Exception exception;

        private readonly Establish context = () =>
        {
            WithdrawMoneyMessageHandler = new WithdrawMoneyMessageHandler(Repository.Object);
            WithdrawMoneyCommand = new WithdrawMoney(AuditInfo, CardNumber, 100);
            Repository.Setup(c => c.Exists<Domain.Aggregate.BankCard>(CardNumber)).Returns(false);
        };
        Because of = () => exception = Catch.Exception(() => WithdrawMoneyMessageHandler.Handle(WithdrawMoneyCommand, Context));
        It should_not_allow_deposit = () => exception.ShouldBeOfExactType<AggregateNotFoundException>();
        It should_report_the_reason = () => exception.Message.ShouldEqual($"Bank card {CardNumber} was not found");
        It should_have_not_saved_bank_card = () => Repository.Verify(foo => foo.Save(Moq.It.IsAny<Domain.Aggregate.BankCard>(), false), Times.Exactly(0));
     }

}
