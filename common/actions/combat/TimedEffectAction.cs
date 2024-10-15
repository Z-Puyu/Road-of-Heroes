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

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            if (this.Effect != null) {
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
                // Success chance = base chance - target resistance
                int successChance = src.Filter(new Stat(
                    chanceType, (int)(flag.HasFlag(ActionFlag.Critical) 
                       ? 50 + this.SuccessChance : this.SuccessChance
                    )
                )).Value - target.Get((StatType)this.Effect.EffectType);
                if (MathUtil.Randi(1, 100) <= successChance) {
                    target.AddEffect(this.Effect);
                }
            }
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target) {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return $"{this.SuccessChance}% chance of {this.Effect}";
        }
    }
}