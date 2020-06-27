using Machine.Specifications;
using Moq;
using NServiceBus.Testing;
using NServiceBus2020.DomainBase;
using NServiceBus2020.DomainBase.Interfaces;
using System;

namespace NServiceBus2020.BankTransactions.UnitTests.Scenarios.Application
{
    public abstract class HandlerSpec
    {
        protected static Mock<IDomainRepository> Repository;
        protected static DateTime DefaultDate = DateTime.UtcNow;
        protected static TestableMessageHandlerContext Context;
        protected static Domain.Aggregate.BankCard SavedBankCard;
        protected static string CardNumber;

   
        protected static AuditInfo AuditInfo = new AuditInfo
        {
            Created = DefaultDate,
            By = "userName"
        };

        protected Establish context = () =>
        {
            CardNumber = "1111111111111111";
            Context = new TestableMessageHandlerContext();
            Repository = new Mock<IDomainRepository>();
            Repository.Setup(c => c.Save(Moq.It.IsAny<Domain.Aggregate.BankCard>(), Moq.It.IsAny<bool>())).Callback<Domain.Aggregate.BankCard, bool>((obj, isInitial)
                => SavedBankCard =obj
            );

        };
        private Cleanup after = () => { };
    }

}