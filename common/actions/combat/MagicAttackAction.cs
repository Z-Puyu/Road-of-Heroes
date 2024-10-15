using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions.combat {
    [RegisteredType(nameof(MagicAttackAction), "", nameof(Resource)), GlobalClass]
    public partial class MagicAttackAction : CombatAction {
        [Export] private uint MinDamage { get; set; } = 0;
        [Export] private uint MaxDamage { get; set; } = 0;

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            // Compute the base damage.
            int dmg = (int)Math.Round(
                flag.HasFlag(ActionFlag.Critical) 
                    ? this.MaxDamage * 1.5
                    : MathUtil.Randi(this.MinDamage, this.MaxDamage)
            );
            // Modify the damage dealt based on attacker's modifiers.
            int dmgDealt = src.Filter(new Stat(StatType.MagicDamageDealt, dmg)).Value;
            // Modify the damage taken based on target's modifiers.
            int dmgReceived = target.Filter(new Stat(
                StatType.MagicDamageTaken, dmgDealt)
            ).Value;
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