using System.Threading.Tasks;
using Game.common.actions;
using Game.common.actions.combat;
using Game.common.characters;
using Game.common.stats;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(CombatEffectAction), "", nameof(Resource)), GlobalClass]
    public partial class CombatEffectAction : CombatAction {
        [Export] private CombatEffect Effect { set; get; }
        [Export] private uint SuccessChance { set; get; } = 100;

        private int ProjectChance(Actor src, Actor target, bool isCritical) {
            StatType chanceType = this.Effect.EffectType switch {
                CombatEffect.Type.Bleed => StatType.BleedChance,
                CombatEffect.Type.Burn => StatType.BurnChance,
                CombatEffect.Type.Poison => StatType.PoisonChance,
                CombatEffect.Type.Stun => StatType.StunChance,
                CombatEffect.Type.Blight => StatType.BlightChance,
                CombatEffect.Type.Frenzy => StatType.FrenzyChance,
                CombatEffect.Type.Buff => StatType.BuffChance,
                CombatEffect.Type.Debuff => StatType.DebuffChance,
                _ => StatType.BleedChance
            };
            int successChance = src.Filter(new Stat(
                chanceType, (int)this.SuccessChance
            )).Value - target.Get((StatType)this.Effect.EffectType);
            return isCritical ? successChance + 50 : successChance;
        }

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            if (CombatAction.ShouldChangeTarget(src, target)) {
                return this.Apply(src, target.Warder, flag);
            }
            if (this.Effect != null) {
                if (MathUtil.Randi(1, 100) <= this.ProjectChance(
                    src, target, flag.HasFlag(ActionFlag.Critical)
                )) {
                    target.AddEffect(src, this.Effect);
                }
            }
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target) {
            return $"{this.ProjectChance(src, target, false)}% chance:\n{this.Effect}";
        }

        public override string ToString() {
            return $"{this.SuccessChance}% base chance:\n{this.Effect}";
        }
    }
}