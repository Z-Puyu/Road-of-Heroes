using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(StatModifier), "", nameof(Resource)), GlobalClass]
    public partial class StatModifier : Modifier {
        [Export] public Stat.Category TargetStat { get; set; }

        public StatModifier() : base() {}
    }
}