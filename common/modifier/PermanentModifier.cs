using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(Modifier), "", nameof(Resource)), GlobalClass]
    public partial class PermanentModifier : Modifier {
        public PermanentModifier() : base(-1) {}
    }
}