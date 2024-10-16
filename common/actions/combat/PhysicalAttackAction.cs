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

        private (int, int) ProjectDamage(Actor src, Actor target, bool isCritical) {
            int strength = src.Get(StatType.Strength);
            StatType typeDealt = this.IsMelee ? StatType.MeleeDamageDealt : StatType.RangedDamageDealt;
            StatType typeTaken = this.IsMelee ? StatType.MeleeDamageTaken : StatType.RangedDamageTaken;
            if (isCritical) {
                int dmg = (int)Math.Round(3 * strength * this.DamageMultiplier / 100.0);
                int dmgDealt = src.Filter(new Stat(typeDealt, dmg)).Value;
                int dmgReceived = target.Filter(new Stat(typeTaken, dmgDealt)).Value;
                return (dmgReceived, dmgReceived);
            } else {
                double min = strength * this.DamageMultiplier / 100.0;
                double max = min * 2;
                (int, int) dmgDealt = (
                    src.Filter(new Stat(typeDealt, (int)Math.Round(min))).Value, 
                    src.Filter(new Stat(typeDealt, (int)Math.Round(max))).Value
                );
                return (
                    target.Filter(new Stat(typeTaken, dmgDealt.Item1)).Value, 
                    target.Filter(new Stat(typeTaken, dmgDealt.Item2)).Value
                );
            }
        }

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            if (CombatAction.ShouldChangeTarget(src, target)) {
                return this.Apply(src, target.Warder, flag);
            }
            // Update HP.
            target.Update(StatType.Health, -MathUtil.Randi(
                this.ProjectDamage(src, target, flag.HasFlag(ActionFlag.Critical))
            ));
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target) {
            (int, int) dmg = this.ProjectDamage(src, target, false);
            string dmgType = this.IsMelee ? "melee" : "ranged";
            return $"{dmg.Item1} - {dmg.Item2} {dmgType} damage";
        }
    }
}