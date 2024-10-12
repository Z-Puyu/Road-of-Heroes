using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Game.util;
using Game.util.events;
using Game.util.events.battle;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(PhysicalAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class PhysicalAttackEffect : CombatEffect {
        [Export] private int DamageMultiplier { get; set; } = 100;
        [Export] private bool IsMelee { get; set; } = true;

        public override void Apply(Actor src, Actor target, bool crit = false) {
            int strength = src.Get(StatType.Strength).Value;
            //this.Publish(new AttackEvent(
            //    src, target, this.IsMelee ? StatType.MeleeDamageDealt 
            //    : StatType.RangedDamageDealt, strength, strength * 2, crit, this.DamageMultiplier
            //));
        }

        public override string GetDesc(Actor src, Actor target) {
            string attack = this.IsMelee ? "melee damage" : "ranged damage";
            int strength = src.Get(StatType.Strength).Value;
            int min = (int)Math.Ceiling(strength * this.DamageMultiplier / 100.0);
            int max = (int)Math.Ceiling(strength * 2 * this.DamageMultiplier / 100.0);
            return $"{(min == max ? $"{min}" : $"{min} to {max}")} {attack}";
        }
    }
}