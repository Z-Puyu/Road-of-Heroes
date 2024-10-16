using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Game.util.enums;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions.combat {
    [RegisteredType(nameof(HealingAction), "", nameof(Resource)), GlobalClass]
    public partial class HealingAction : CombatAction {
        private enum Target {
            HP = StatType.Health, 
            Magicka = StatType.Magicka, 
            Sanity = StatType.Sanity, 
            Stamina = StatType.Stamina
        }

        [Export] private uint MinAmount { get; set; } = 0;
        [Export] private uint MaxAmount { get; set; } = 0;
        [Export] private Target TargetStat { get; set; } = Target.HP;
        [Export] private uint CriticalChance { get; set; } = 5;

        private (int, int) ProjectAmount(Actor src, Actor target, bool isCritical) {
            (StatType, StatType) healType = this.TargetStat switch {
                Target.HP => (StatType.HpHealingGiven, StatType.HpHealingReceived),
                Target.Magicka => (StatType.MagickaHealingGiven, StatType.MagickaHealingReceived),
                Target.Sanity => (StatType.SanityHealingGiven, StatType.SanityHealingReceived),
                Target.Stamina => (StatType.StaminaHealingGiven, StatType.StaminaHealingReceived),
                _ => (StatType.HpHealingGiven, StatType.HpHealingReceived)
            };
            if (isCritical) {
                int amount = (int)Math.Round(1.5 * this.MaxAmount);
                int healingGiven = src.Filter(new Stat(healType.Item1, amount)).Value;
                int healingReceived = target.Filter(new Stat(
                    healType.Item2, healingGiven)
                ).Value;
                return (healingReceived, healingReceived);
            } else {
                (int, int) healingGiven = (
                    src.Filter(new Stat(healType.Item1, (int)this.MinAmount)).Value,
                    src.Filter(new Stat(healType.Item1, (int)this.MaxAmount)).Value
                );
                return (
                    target.Filter(new Stat(healType.Item2, healingGiven.Item1)).Value,
                    target.Filter(new Stat(healType.Item2, healingGiven.Item2)).Value
                );
            }
        }

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            // Compute the base healing amount.
            bool isCritical = MathUtil.Randi(1, 100) <= this.CriticalChance;
            // Update stats.
            target.Update((StatType)this.TargetStat, MathUtil.Randi(
                this.ProjectAmount(src, target, isCritical)
            ));
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target) {
            (int, int) healRange = this.ProjectAmount(src, target, false);
            return $"Restores {healRange.Item1} - {healRange.Item2} {((StatType)this.TargetStat).ToText()}";
        }
    }
}