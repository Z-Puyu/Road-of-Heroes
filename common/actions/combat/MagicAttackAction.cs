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

        private (int, int) ProjectDamage(Actor src, Actor target, bool isCritical) {
            if (isCritical) {
                int dmg = (int)Math.Round(1.5 * this.MaxDamage);
                int dmgDealt = src.Filter(new Stat(StatType.MagicDamageDealt, dmg)).Value;
                int dmgReceived = target.Filter(new Stat(StatType.MagicDamageTaken, dmgDealt)).Value;
                return (dmgReceived, dmgReceived);
            } else {
                (int, int) dmgDealt = (
                    src.Filter(new Stat(StatType.MagicDamageDealt, (int)this.MinDamage)).Value, 
                    src.Filter(new Stat(StatType.MagicDamageDealt, (int)this.MaxDamage)).Value
                );
                return (
                    target.Filter(new Stat(StatType.MagicDamageTaken, dmgDealt.Item1)).Value, 
                    target.Filter(new Stat(StatType.MagicDamageTaken, dmgDealt.Item2)).Value
                );
            }
        }

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            // Update HP.
            target.Update(StatType.Health, -MathUtil.Randi(
                this.ProjectDamage(src, target, flag.HasFlag(ActionFlag.Critical))
            ));
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target) {
            (int, int) dmg = this.ProjectDamage(src, target, false);
            return $"{dmg.Item1} - {dmg.Item2} magic damage";
        }
    }
}