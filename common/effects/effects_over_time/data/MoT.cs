using Game.common.modifier;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.effects.eot {
    [RegisteredType(nameof(MoT), "", nameof(Resource)), GlobalClass]
    public partial class MoT : EoT {
        [Export] public Array<Modifier> Modifiers { set; get; }
    }
}