using Godot;

namespace Game.common.stats {
    public abstract partial class CappedGameValue : GameValue {
        protected int MaxValue { set; get; }

        protected CappedGameValue() : base() {}

        protected CappedGameValue(int value, int maxValue) : base(value) {
            this.MaxValue = maxValue;
        }
    }
}