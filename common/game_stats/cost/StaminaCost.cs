using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(StaminaCost), "", nameof(Resource)), GlobalClass]
    public class StaminaCost : Cost {
        public StaminaCost(int value) : base(StatType.StaminaCost, value) {}

        public override string ToString() {
            return $"(this.IsPercentage ? {this.Value}% of maximum : {this.Value}) Stamina.";
        }
    }
}