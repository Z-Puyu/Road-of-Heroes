using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects.eot {
    [RegisteredType(nameof(DoT), "", nameof(Resource)), GlobalClass]
    public partial class DoT : EoT {
        [Export] public int Amount { set; get; } = 1;

        public override string ToString() {
            return $"{this.Amount} {this.EffectType} damage for {this.TimeToLast} turns";
        }
    }
}