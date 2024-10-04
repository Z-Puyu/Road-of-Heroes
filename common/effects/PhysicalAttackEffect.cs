using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(PhysicalAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class PhysicalAttackEffect : Effect<Actor, Actor> {
        [Export] private int DamageMultiplier { get; set; } = 100;
        [Export] private bool IsMelee { get; set; } = true;

        public override async Task Apply(Actor src, Actor target, bool crit = false) {
            src.Attack(
                this.IsMelee ? StatType.MeleeDamageDealt : StatType.RangedDamageDealt, 
                target, crit, this.DamageMultiplier
            );
        }

        public override string ToDesc(Actor actor) {
            string attack = this.IsMelee ? "melee damage" : "ranged damage";
            int strength = actor.Get(StatType.Strength).Value;
            int min = (int)Math.Ceiling(strength * this.DamageMultiplier / 100.0);
            int max = (int)Math.Ceiling(strength * 2 * this.DamageMultiplier / 100.0);
            return $"{(min == max ? $"{min}" : $"{min} to {max}")} {attack}";
        }
    }
}