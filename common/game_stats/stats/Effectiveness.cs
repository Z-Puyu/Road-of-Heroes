using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    /// <summary>
    /// Encapsulates the "powerful-ness" of an effect.
    /// </summary>
    [RegisteredType(nameof(Effectiveness), "", nameof(Resource)), GlobalClass]
    public class Effectiveness : Stat {
        public Effectiveness() : base(StatType.Effectiveness, 100) {}
    }
}