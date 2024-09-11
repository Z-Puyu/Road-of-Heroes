using Game.common.modifier;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects.eot {
    [RegisteredType(nameof(MoT), "", nameof(Resource)), GlobalClass]
    public partial class MoT : EoT {
        [Export] public Modifier Modifier { set; get; }

        public override string ToString() {
            return $"{this.Modifier} for {this.TimeToLast} turns";
        }
    }
}