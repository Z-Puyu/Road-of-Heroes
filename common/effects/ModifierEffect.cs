using Game.common.effects.eot;
using Game.common.tokens;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(ModifierEffect), "", nameof(Resource)), GlobalClass]
    public partial class ModifierEffect : Effect {
        [Export] private MoT Buff { set; get; }
        [Export] private MoT Debuff { set; get; }
        [Export] private int SuccessChance { set; get; } = 100;

        public override async void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (target is not CharacterCard receiver || this.EffectType != Type.Modifier) {
                return;
            }
            int dice = Utilities.Randi(1, 100);
            if (this.Buff != null && this.Buff.EffectType == EoT.Effect.Buff) {
                if (dice <= this.SuccessChance) {
                    await FloatingCaption.Node.Display(receiver.GlobalPosition, this.Buff.EffectType);
                    receiver.GetNode<EoTManager>(receiver.EoTManager).Add(this.Buff);
                } else {
                    await FloatingCaption.Node.Display(receiver.GlobalPosition, this.Buff.EffectType, true);
                }
            }
            if (this.Debuff != null && this.Debuff.EffectType == EoT.Effect.Debuff) {
                if (dice <= this.SuccessChance) {
                    await FloatingCaption.Node.Display(receiver.GlobalPosition, this.Debuff.EffectType);
                    receiver.GetNode<EoTManager>(receiver.EoTManager).Add(this.Debuff);
                } else {
                    await FloatingCaption.Node.Display(receiver.GlobalPosition, this.Debuff.EffectType, true);
                }
            }
        }
    }
}