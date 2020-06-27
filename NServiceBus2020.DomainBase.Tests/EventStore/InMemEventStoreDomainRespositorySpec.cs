﻿using Machine.Specifications;
using Moq;
using NServiceBus2020.DomainBase.Interfaces;
using NServiceBus2020.DomainBase.Repository;
using NServiceBus2020.DomainBase.Tests.Domain.TestEntityAggregate;
using System;
using It = Machine.Specifications.It;

namespace NServiceBus2020.DomainBase.Tests.EventStore
{

    public abstract class InMemEventStoreDomainRespositorySpec
    {
        protected static InMemEventStoreDomainRespository SUT;
        protected static Mock<ITransientDomainEventPublisher> TransientDomainEventPublisherMock;
        protected static System.Exception _exception;
        readonly Establish context = () =>
        {
            TransientDomainEventPublisherMock = new Mock<ITransientDomainEventPublisher>();
            SUT = new InMemEventStoreDomainRespository("TestCategory", TransientDomainEventPublisherMock.Object);
            
        };
    }

    [Subject(typeof(InMemEventStoreDomainRespository))]
    public class when_saving_a_new_aggregate: InMemEventStoreDomainRespositorySpec
    {
        protected static TestEntityAggregate TestEntityAggregate;
        protected static string Id;
        readonly Establish context = () =>
        {
            TestEntityAggregate = new TestEntityAggregate();
            Id = string.Format("CreateId-{0}", Guid.NewGuid());
            TestEntityAggregate.Create(new AuditInfo(), Id);
        };
        readonly Because of = () => _exception = Catch.Exception(() => SUT.Save(TestEntityAggregate));
        readonly It should_have_not_raised_any_errors = () => _exception.ShouldBeNull();
        readonly It should_saved_successfully = () => SUT.GetById<TestEntityAggregate>(Id).ShouldNotBeNull();
        readonly It should_saved_the_id_successfully = () => SUT.GetById<TestEntityAggregate>(Id).Id.ShouldNotBeNull();
        readonly It should_publish_the_event= () => TransientDomainEventPublisherMock.Verify(foo=>foo.PublishAsync(Moq.It.IsAny<object>()),Times.AtLeastOnce);
    }


    [Subject(typeof(InMemEventStoreDomainRespository))]
    public class WhenSaving10InMemEventsToTheAggregate : InMemEventStoreDomainRespositorySpec
    {
        protected static TestEntityAggregate TestEntityAggregate;
        protected static string Id;
        protected static int CountLength = 10;

        private readonly Establish context = () =>
        {
            var auditInfo = new AuditInfo();
            TestEntityAggregate = new TestEntityAggregate();
            Id = string.Format("MultipleTest10Id-{0}", Guid.NewGuid());
            TestEntityAggregate.Create(auditInfo, Id);
            for (int i = 1; i <= CountLength; i++)
            {
                TestEntityAggregate.Update(auditInfo, Id, string.Format("Test string value {0}", i));
            }
        };
        readonly Because of = () => _exception = Catch.Exception(() => SUT.Save(TestEntityAggregate));
        readonly It should_have_not_raised_any_errors = () => _exception.ShouldBeNull();
        readonly It should_saved_successfully = () => SUT.GetById<TestEntityAggregate>(Id).ShouldNotBeNull();
        readonly It should_saved_the_id_successfully = () => SUT.GetById<TestEntityAggregate>(Id).Id.ShouldNotBeNull();
        readonly It should_publish_all_the_events = () => TransientDomainEventPublisherMock.Verify(foo => foo.PublishAsync(Moq.It.IsAny<object>()), Times.Exactly(CountLength + 1));
    }


    [Subject(typeof(InMemEventStoreDomainRespository))]
    public class WhenSavingOver200InMemEventsToTheAggregate : InMemEventStoreDomainRespositorySpec
    {
        protected static TestEntityAggregate TestEntityAggregate;
        protected static string Id;
        protected static int CountLength =201;

        private readonly Establish context = () =>
        {
            var auditInfo= new AuditInfo();
            TestEntityAggregate = new TestEntityAggregate();
            Id = string.Format("MultipleTest200PlusId-{0}",Guid.NewGuid());
            TestEntityAggregate.Create(auditInfo, Id);
            for (int i = 1; i <= CountLength; i++)
            {
                TestEntityAggregate.Update(auditInfo,Id,string.Format("Test string value {0}",i));
            }
        };
        readonly Because of = () => _exception = Catch.Exception(() => SUT.Save(TestEntityAggregate));
        readonly It should_have_not_raised_any_errors = () => _exception.ShouldBeNull();
        readonly It should_saved_successfully = () => SUT.GetById<TestEntityAggregate>(Id).ShouldNotBeNull();
        readonly It should_publish_all_the_events = () => TransientDomainEventPublisherMock.Verify(foo => foo.PublishAsync(Moq.It.IsAny<object>()), Times.Exactly(CountLength+1));
    }


    [Subject(typeof(InMemEventStoreDomainRespository))]
    public class WhenHandlingOverInMemEventsLimitOfTheAggregate : InMemEventStoreDomainRespositorySpec
    {
        protected static TestEntityAggregate TestEntityAggregate;
        protected static string Id;
        protected static int CountLength = 5000;

        private readonly Establish context = () =>
        {
            var auditInfo = new AuditInfo();
            TestEntityAggregate = new TestEntityAggregate();
            Id = string.Format("OverEventReadLimitCountId-{0}", Guid.NewGuid());
            TestEntityAggregate.Create(auditInfo, Id);
            for (int i = 1; i <= CountLength; i++)
            {
                TestEntityAggregate.Update(auditInfo, Id, string.Format("Test string value {0}", i));
            }
        };
        readonly Because of = () => _exception = Catch.Exception(() => SUT.Save(TestEntityAggregate));
        readonly It should_have_not_raised_any_errors = () => _exception.ShouldBeNull();
        readonly It should_saved_successfully = () => SUT.GetById<TestEntityAggregate>(Id).ShouldNotBeNull();
        readonly It should_publish_all_the_events = () => TransientDomainEventPublisherMock.Verify(foo => foo.PublishAsync(Moq.It.IsAny<object>()), Times.Exactly(CountLength + 1));
    }
}