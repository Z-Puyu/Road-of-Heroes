using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
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

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            // Compute the base healing amount.
            bool isCritical = MathUtil.Randi(1, 100) <= this.CriticalChance;
            int amount = (int)Math.Round(
                isCritical ? this.MaxAmount * 1.5 : MathUtil.Randi(this.MinAmount, this.MaxAmount)
            );
            (StatType, StatType) healType = this.TargetStat switch {
                Target.HP => (StatType.HpHealingGiven, StatType.HpHealingReceived),
                Target.Magicka => (StatType.MagickaHealingGiven, StatType.MagickaHealingReceived),
                Target.Sanity => (StatType.SanityHealingGiven, StatType.SanityHealingReceived),
                Target.Stamina => (StatType.StaminaHealingGiven, StatType.StaminaHealingReceived),
                _ => (StatType.HpHealingGiven, StatType.HpHealingReceived)
            };
            // Modify the healing given based on healer's modifiers.
            int healingGiven = src.Filter(new Stat(healType.Item1, amount)).Value;
            // Modify the healing received based on target's modifiers.
            int healingReceived = target.Filter(new Stat(
                healType.Item2, healingGiven)
            ).Value;
            // Update stats.
            target.Update((StatType)this.TargetStat, healingReceived);
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target)
        {
            throw new NotImplementedException();
        }
    }
}