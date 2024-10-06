using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(MagickaCost), "", nameof(Resource)), GlobalClass]
    public class MagickaCost : Cost {
        public MagickaCost(int value) : base(StatType.MagickaCost, value) {}

        public override string ToString() {
            return $"(this.IsPercentage ? {this.Value}% of maximum: {this.Value}) Magicka.";
        }
    }
}