using Protoinject;
using Prototest.Library.Version1;

namespace Protogame.Tests
{
    public class EventEngineTests
    {
        private static IAssert _staticAssert;

        private class BasicEvent : Event
        {
            public int Original { get; set; }
            public int HitValue { get; set; }
        }
        
        private class OtherEvent : Event
        {
        }
        
        private class BasicAction : IEventAction<IGameContext>
        {
            public void Handle(IGameContext gameContext, Event @event)
            {
                var basicEvent = @event as BasicEvent;
                basicEvent.HitValue = basicEvent.Original;
            }
        }
        
        private class BasicAssertingAction : IEventAction<IGameContext>
        {
            public void Handle(IGameContext gameContext, Event @event)
            {
                _staticAssert.IsType<BasicEvent>(@event);
            }
        }
        
        private class BasicStaticEventBinder : StaticEventBinder<IGameContext>
        {
            public override void Configure()
            {
                this.Bind<BasicEvent>(x => true).To<BasicAction>();
            }
        }
        
        private class AssertingStaticEventBinder : StaticEventBinder<IGameContext>
        {
            public override void Configure()
            {
                this.Bind<BasicEvent>(x => true).To<BasicAssertingAction>();
            }
        }
        
        private class FilteredStaticEventBinder : StaticEventBinder<IGameContext>
        {
            public override void Configure()
            {
                this.Bind<BasicEvent>(x => x.Original == 1).To<BasicAction>();
            }
        }

        public EventEngineTests(IAssert assert)
        {
            _staticAssert = assert;
        }
    
        public void TestBasicPropagation()
        {
            var @event = new BasicEvent { Original = 1 };
            var binder = new BasicStaticEventBinder();
            var engine = new DefaultEventEngine<IGameContext>(new StandardKernel(), new[] { binder });
            engine.Fire(null, @event);
            _staticAssert.Equal(1, @event.HitValue);
        }
    
        public void TestFilteredPropagationInOrder()
        {
            var event1 = new BasicEvent { Original = 1 };
            var event2 = new BasicEvent { Original = 2 };
            var binder = new FilteredStaticEventBinder();
            var engine = new DefaultEventEngine<IGameContext>(new StandardKernel(), new[] { binder });
            engine.Fire(null, event1);
            _staticAssert.Equal(1, event1.HitValue);
            engine.Fire(null, event2);
            _staticAssert.NotEqual(2, event2.HitValue);
        }
        
        public void TestFilteredPropagationOutOfOrder()
        {
            var event1 = new BasicEvent { Original = 1 };
            var event2 = new BasicEvent { Original = 2 };
            var binder = new FilteredStaticEventBinder();
            var engine = new DefaultEventEngine<IGameContext>(new StandardKernel(), new[] { binder });
            engine.Fire(null, event2);
            _staticAssert.NotEqual(2, event2.HitValue);
            engine.Fire(null, event1);
            _staticAssert.Equal(1, event1.HitValue);
        }
        
        public void TestEventTypeCheck()
        {
            var @event = new OtherEvent();
            var binder = new AssertingStaticEventBinder();
            var engine = new DefaultEventEngine<IGameContext>(new StandardKernel(), new[] { binder });
            engine.Fire(null, @event);
        }
    }
}

