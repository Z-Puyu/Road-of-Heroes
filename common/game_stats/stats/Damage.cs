using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Damage), "", nameof(Resource)), GlobalClass]
    public partial class Damage : ModifiableValue {
        public Damage(ModifiableValueType t, int amount) : base(t, amount) {}
    }
}