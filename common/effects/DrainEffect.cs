using System;
using System.Collections.Generic;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(DrainEffect), "", nameof(Resource)), GlobalClass]
    public partial class DrainEffect : Effect {
        private readonly static Dictionary<Type, Stat.Category> stats = new
                Dictionary<Type, Stat.Category> {
            {Type.MagickaDrain, Stat.Category.Magicka},
            {Type.SanityDrain, Stat.Category.Sanity},
            {Type.FatigueLoss, Stat.Category.Fatigue}
        };

        [Export] private bool IsPercentage { set; get; } = false;
        [Export] private Vector2I DrainRange { set; get; } = Vector2I.Zero;

        public override void Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not Character || target is not Character receiver || 
                    (this.EffectType != Type.SanityDrain && this.EffectType != Type.MagickaDrain && 
                     this.EffectType != Type.FatigueLoss)) {
                return;
            }
            if (DrainEffect.stats.TryGetValue(this.EffectType, out Stat.Category stat)) {
                int amount;
                if (this.IsPercentage && receiver.Get(stat, out Stat value)) {
                    amount = (int)Math.Round(
                        Utilities.Randi(this.DrainRange.X, this.DrainRange.Y) * 
                        value.MaxValue / 100.0
                    );
                } else {
                    amount = Utilities.Randi(this.DrainRange.X, this.DrainRange.Y);
                }
                receiver.Update(
                    stat, target.Receive(this.EffectType, src.Emit(this.EffectType, -amount))
                );
            }
        }

        public override string ToString() {
            string stat = this.EffectType switch {
                Type.SanityDrain => "sanity",
                Type.MagickaDrain => "magicka",
                Type.FatigueLoss => "fatigue",
                _ => ""
            };
            if (this.IsPercentage) {
                return $"Drain {(this.DrainRange.X == this.DrainRange.Y 
                    ? $"{this.DrainRange.X}%" 
                    : $"{this.DrainRange.X}% to {this.DrainRange.Y}%")} of maximum {stat}";
            }
            return $"Drain {(this.DrainRange.X == this.DrainRange.Y 
                    ? $"{this.DrainRange.X}" 
                    : $"{this.DrainRange.X} to {this.DrainRange.Y}")} {stat}";
        }
    }
}