using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.util;
using Game.util.events.battle;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(DisplaceEffect), "", nameof(Resource)), GlobalClass]
    public partial class DisplaceEffect : Effect {
        [Export] private int SuccessChance { set; get; }
        [Export] private int StepSize { set; get; } = 1;

        public override async Task Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (target is not CharacterCard receiver || this.EffectType != Type.Displace) {
                return;
            }
            int dice = Utilities.Randi(1, 100);
            if (dice <= this.SuccessChance) {
                this.Publish(new DisplaceCharacterEvent(receiver, this.StepSize));
            }
            await Task.CompletedTask;
        }

        public override string ToString() {
            return $"{this.SuccessChance}% chance to " 
                 + $"{(this.StepSize > 0 ? "step back" : "step forward")} " 
                 + $"by {Math.Abs(this.StepSize)} steps";
        }
    }
}