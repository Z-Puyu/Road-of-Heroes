using Game.common.modifier;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions {
    [RegisteredType(nameof(TimedModifier), "", nameof(Resource)), GlobalClass]
    public partial class TimedModifier : Resource {
        [Export] public Modifier Modifier { get; private set; }
        [Export(PropertyHint.Range, "0, 1000")] public int Duration { get; private set; }
    }
}