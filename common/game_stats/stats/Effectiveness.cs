using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    /// <summary>
    /// Encapsulates the "powerful-ness" of an effect.
    /// </summary>
    [RegisteredType(nameof(Effectiveness), "", nameof(Resource)), GlobalClass]
    public partial class Effectiveness : ModifiableValue {
        public Effectiveness() : base(ModifiableValueType.Effectiveness, 100) {}
    }
}