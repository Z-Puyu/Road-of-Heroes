using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Heal), "", nameof(Resource)), GlobalClass]
    public partial class Heal : Stat {
        public StatType TargetStat { set; get; }
        public Heal(StatType targetStat, int amount) : base(targetStat switch {
            StatType.Health => StatType.HpHeal,
            StatType.Magicka => StatType.MagickaHeal,
            StatType.Sanity => StatType.SanityHeal,
            StatType.Stamina => StatType.StaminaHeal,
            _ => StatType.HpHeal
        }, amount) {
            this.TargetStat = targetStat;
        }
    }
}