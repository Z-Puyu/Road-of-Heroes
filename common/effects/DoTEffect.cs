using Game.common.characters;
using Game.common.effects.eot;
using Game.common.tokens;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(DoTEffect), "", nameof(Resource)), GlobalClass]
    public partial class DoTEffect : Effect {
        private readonly static Dictionary<EoT.Effect, Stat.Category> resistance =
                new Dictionary<EoT.Effect, Stat.Category> {
            {EoT.Effect.Bleed, Stat.Category.BleedResist},
            {EoT.Effect.Blight, Stat.Category.BlightResist},
            {EoT.Effect.Burn, Stat.Category.BurnResist},
            {EoT.Effect.Poison, Stat.Category.PoisonResist},
            {EoT.Effect.Stun, Stat.Category.StunResist},
        };

        [Export] private DoT DoT { set; get; }
        [Export] private int SuccessChance { set; get; } = 100;

        public override async void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (target is not CharacterCard receiver || this.EffectType != Type.DoT) {
                return;
            }
            if (this.DoT != null && DoTEffect.resistance.TryGetValue(
                this.DoT.EffectType, out Stat.Category resistance
            )) {
                int dice = Utilities.Randi(1, 100);
                if (dice <= this.SuccessChance - receiver.Character.Get(resistance)) {
                    await FloatingCaption.Node.Display(receiver.GlobalPosition, this.DoT.EffectType);
                    receiver.GetNode<EoTManager>(receiver.EoTManager).Add(this.DoT);
                } else {
                    await FloatingCaption.Node.Display(receiver.GlobalPosition, this.DoT.EffectType, true);
                }
            }
        }
    }
}