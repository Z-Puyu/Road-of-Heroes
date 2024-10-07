using Godot;

namespace Game.common.stats {
    public abstract partial class GameValue : Resource {
        [Export] protected int Value { set; get; } = 0;

        protected GameValue() {}

        protected GameValue(int value) {
            this.Value = value;
        }
    }
}