using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Damage), "", nameof(Resource)), GlobalClass]
    public partial class Damage : Stat {
        public Damage(StatType t, int amount) : base(t, amount) {}
    }
}