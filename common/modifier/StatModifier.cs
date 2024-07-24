using Game.common.characters;
using Godot;

namespace Game.common.modifier {
    public partial class StatModifier : Modifier {
        [Export] public Stat.Category TargetStat { get; set; }

        public StatModifier() : base() {}
    }
}