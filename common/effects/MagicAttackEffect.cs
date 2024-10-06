using Game.common.characters;
using Game.common.stats;
using Game.util;
using Game.util.events.battle;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(MagicAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class MagicAttackEffect : CombatEffect {
        [Export] private int MinDamage { set; get; }
        [Export] private int MaxDamage { set; get;}

        public override void Apply(Actor src, Actor target, bool crit = false) {
            this.Publish(new AttackEvent(
                src, target, StatType.MagicDamageDealt, this.MinDamage, this.MaxDamage, crit
            ));
        }

        public override string ToString() {
            return this.MinDamage == this.MaxDamage
                    ? $"{this.MinDamage} magic damage" 
                    : $"{this.MinDamage} to {this.MaxDamage} magic damage";
        }

        public override string GetDesc(Actor src, Actor target) {
            return this.ToString();
        }
    }
}