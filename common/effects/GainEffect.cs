using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(GainEffect), "", nameof(Resource)), GlobalClass]
    public partial class GainEffect : Effect {
        private readonly static Dictionary<Type, Stat.Category> stats = new
                Dictionary<Type, Stat.Category> {
            {Type.MagickaRestore, Stat.Category.Magicka},
            {Type.SanityRestore, Stat.Category.Sanity},
            {Type.FatigueGain, Stat.Category.Fatigue}
        };

        [Export] private bool IsPercentage { set; get; } = false;
        [Export] private Vector2I GainRange { set; get; } = Vector2I.Zero;

        public override async Task Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character || target is not CharacterCard card || 
                    (this.EffectType != Type.SanityRestore && 
                     this.EffectType != Type.MagickaRestore && 
                     this.EffectType != Type.FatigueGain)) {
                return;
            }
            Character receiver = card.Character;
            if (GainEffect.stats.TryGetValue(this.EffectType, out Stat.Category stat)) {
                int amount;
                if (this.IsPercentage && receiver.Get(stat, out Stat value)) {
                    amount = (int)Math.Round(
                        Utilities.Randi(this.GainRange.X, this.GainRange.Y) * 
                        value.MaxValue / 100.0
                    );
                } else {
                    amount = Utilities.Randi(this.GainRange.X, this.GainRange.Y);
                }
                await FloatingCaption.Node.Display(stat, amount, card.GlobalPosition, crit, true);
                receiver.Update(
                    stat, target.Receive(this.EffectType, src.Emit(this.EffectType, amount))
                );
            }
        }

        public override string ToString() {
            string stat = this.EffectType switch {
                Type.SanityRestore => "sanity",
                Type.MagickaRestore => "magicka",
                Type.FatigueGain => "fatigue",
                _ => ""
            };
            if (this.IsPercentage) {
                return $"Gain {(this.GainRange.X == this.GainRange.Y 
                    ? $"{this.GainRange.X}%" 
                    : $"{this.GainRange.X}% to {this.GainRange.Y}%")} of maximum {stat}";
            }
            return $"Gain {(this.GainRange.X == this.GainRange.Y 
                    ? $"{this.GainRange.X}" 
                    : $"{this.GainRange.X} to {this.GainRange.Y}")} {stat}";
        }
    }
}