using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects.eot {
    [RegisteredType(nameof(Stun), "", nameof(Resource)), GlobalClass]
    public partial class Stun : EoT {
        public override string ToString() {
            return "Stun the target in the next turn";
        }
    }
}