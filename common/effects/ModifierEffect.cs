using System;
using System.Linq;
using Game.common.characters;
using Game.common.effects.eot;
using Game.common.tokens;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(ModifierEffect), "", nameof(Resource)), GlobalClass]
    public partial class ModifierEffect : Effect {
        [Export] private Array<MoT> Buffs { set; get; } = [];
        [Export] private Array<MoT> Debuffs { set; get; } = [];
        [Export] private int SuccessChance { set; get; } = 100;

        public override async void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (target is not CharacterCard receiver || this.EffectType != Type.Modifier) {
                return;
            }
            int dice = Utilities.Randi(1, 100);
            foreach (MoT buff in this.Buffs) {
                if (buff.EffectType == EoT.Effect.Buff) {
                    if (dice <= this.SuccessChance) {
                        await FloatingCaption.Node.Display(receiver.GlobalPosition, buff.EffectType);
                        receiver.GetNode<EoTManager>(receiver.EoTManager).Add(buff);
                    } else {
                        await FloatingCaption.Node.Display(receiver.GlobalPosition, buff.EffectType, true);
                    }
                }
            }
            foreach (MoT debuff in this.Debuffs) {
                if (debuff.EffectType == EoT.Effect.Debuff) {
                    if (dice <= this.SuccessChance) {
                        await FloatingCaption.Node.Display(receiver.GlobalPosition, debuff.EffectType);
                        receiver.GetNode<EoTManager>(receiver.EoTManager).Add(debuff);
                    } else {
                        await FloatingCaption.Node.Display(receiver.GlobalPosition, debuff.EffectType, true);
                    }
                }
            }
        }

        public override string ToString() {
            return $"{this.SuccessChance}% chance:\n" 
                 + $"{string.Join('\n', this.Buffs.Concat(this.Debuffs).Select(x => x.ToString()))}";
        }
    }
}