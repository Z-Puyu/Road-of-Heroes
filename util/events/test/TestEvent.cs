using System;

namespace Game.util.events.test {
    public class TestEvent : EventArgs {
        public Guid Id { get; private init; } = Guid.NewGuid();
    }
}