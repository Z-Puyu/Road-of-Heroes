using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions.combat {
    [RegisteredType(nameof(PhysicalAttackAction), "", nameof(Resource)), GlobalClass]
    public partial class PhysicalAttackAction : CombatAction {
        [Export] private int DamageMultiplier { get; set; } = 100;
        [Export] private bool IsMelee { get; set; } = true;

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            int strength = src.Get(StatType.Strength);
            //this.Publish(new AttackEvent(
            //    src, target, this.IsMelee ? StatType.MeleeDamageDealt 
            //    : StatType.RangedDamageDealt, strength, strength * 2, crit, this.DamageMultiplier
            //));
        }

        public override Task Apply(Actor src, Actor target, ActionFlag flag) {
            if (flag.HasFlag(ActionFlag.Hit)) {
                
            }
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target)
        {
            throw new NotImplementedException();
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