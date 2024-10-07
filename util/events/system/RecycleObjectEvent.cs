namespace Game.util.events.system {
    public class RecycleObjectEvent : ConditionalEvent<object> {
        public object Object { get; private init; }

        public RecycleObjectEvent(object @object) : base(null, [@object]) {
            this.Object = @object;
        }
    }
}