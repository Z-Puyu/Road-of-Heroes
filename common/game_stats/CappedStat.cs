using Godot;

namespace Game.common.stats {
    [GlobalClass]
    public abstract partial class CappedStat : Stat {
        [Export] public Stat MaxValue { protected set; get; }

        public CappedStat() : base() {}

        public CappedStat(StatType type, Stat maxValue, int value = 0) : base(type, value) {
            this.MaxValue = maxValue;
        }

        public override string ToString() {
            return $"{this.Value} {this.Type}";
        }
    }
}