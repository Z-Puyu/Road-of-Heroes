using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(HpCost), "", nameof(Resource)), GlobalClass]
    public class HpCost : Cost {
        public HpCost(int value) : base(StatType.HpCost, value) {}

        public override string ToString() {
            return $"(this.IsPercentage ? {this.Value}% of maximum : {this.Value}) HP.";
        }
    }
}