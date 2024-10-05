using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Heal), "", nameof(Resource)), GlobalClass]
    public class Heal : Stat {
        public StatType TargetStat { set; get; }
        public Heal(StatType targetStat, StatType t, int amount) : base(t, amount) {
            this.TargetStat = targetStat;
        }
    }
}