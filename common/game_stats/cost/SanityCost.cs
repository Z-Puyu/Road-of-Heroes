using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(SanityCost), "", nameof(Resource)), GlobalClass]
    public class SanityCost : Cost {
        public SanityCost(int value) : base(StatType.SanityCost, value) {}

        public override string ToString() {
            return $"(this.IsPercentage ? {this.Value}% of maximum : {this.Value}) Sanity.";
        }
    }
}