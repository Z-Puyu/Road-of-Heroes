using Godot;

namespace Game.util.events.system {
    public class RecycleObjectEvent : ConditionalEvent<object> {
        public RecycleObjectEvent(object node) : base(null, [node]) {}        
    }
}