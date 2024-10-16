using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions {
    [RegisteredType(nameof(Stun), "", nameof(Resource)), GlobalClass]
    public partial class Stun : CombatEffect {
        public Stun() : base(CombatEffect.Type.Stun) {}

        public override string ToString() {
            return "Stun the target for 1 turn.";
        }
    }
}