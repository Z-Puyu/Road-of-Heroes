using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions.combat {
    [RegisteredType(nameof(PhysicalAttackAction), "", nameof(Resource)), GlobalClass]
    public partial class PhysicalAttackAction : CombatAction {
        [Export] private uint DamageMultiplier { get; set; } = 100;
        [Export] private bool IsMelee { get; set; } = true;

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            // Compute the base damage.
            int strength = src.Get(StatType.Strength);
            int dmg = (int)Math.Round(
                flag.HasFlag(ActionFlag.Critical) 
                    ? 2 * strength * 1.5
                    : MathUtil.Randi(strength, strength * 2) * this.DamageMultiplier / 100.0
            );
            // Modify the damage dealt based on attacker's modifiers.
            int dmgDealt = src.Filter(new Stat(
                this.IsMelee ? StatType.MeleeDamageDealt : StatType.RangedDamageDealt, dmg
            )).Value;
            // Modify the damage taken based on target's modifiers.
            int dmgReceived = target.Filter(new Stat(
                this.IsMelee ? StatType.MeleeDamageTaken : StatType.RangedDamageTaken, 
                dmgDealt
            )).Value;
            // Update HP.
            target.Update(StatType.Health, -dmgReceived);
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target)
        {
            throw new NotImplementedException();
        }
    }
}