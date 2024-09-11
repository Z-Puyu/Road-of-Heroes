using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(HealEffect), "", nameof(Resource)), GlobalClass]
    public partial class HealEffect : Effect {
        [Export] private bool IsPercentage { set; get; } = false;
        [Export] private Vector2I HealRange { set; get; } = Vector2I.Zero;
        [Export] private int CriticalChance { set; get; } = 5;

        public override async Task Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not CharacterCard || target is not CharacterCard card || 
                    (this.EffectType != Type.PhysicalHeal && this.EffectType != Type.MagicHeal)) {
                return;
            }
            Character receiver = card.Character;
            int amount;
            int dice = Utilities.Randi(1, 100);
            if (this.IsPercentage && receiver.Get(Stat.Category.Health, out Stat value)) {
                amount = (int)Math.Round(
                    Utilities.Randi(this.HealRange.X, this.HealRange.Y) * value.MaxValue / 100.0
                );
                if (dice <= this.CriticalChance) {
                    amount = (int)Math.Round(1.5 * amount);
                }
            } else {
                amount = target.Receive(
                    Type.PhysicalHeal, src.Emit(Type.PhysicalHeal, (int)Math.Round(
                        dice <= this.CriticalChance 
                        ? Utilities.Randi(this.HealRange.X, this.HealRange.Y) * 1.5 
                        : Utilities.Randi(this.HealRange.X, this.HealRange.Y)
                    ))
                );
            }
            await FloatingCaption.Node.Display(Stat.Category.Health, amount, card.GlobalPosition, crit, true);
            receiver.Update(Stat.Category.Health, amount);
        }

        public override string ToString() {
            string method = this.EffectType switch {
                Type.PhysicalHeal => "by treatment",
                Type.MagicHeal => "by magic",
                _ => ""
            };
            string amount = this.HealRange.X == this.HealRange.Y 
                              ? $"{this.HealRange.X}" 
                              : $"{this.HealRange.X} to {this.HealRange.Y}";
            if (this.IsPercentage) {
                amount = this.HealRange.X == this.HealRange.Y 
                              ? $"{this.HealRange.X}%" 
                              : $"{this.HealRange.X}% to {this.HealRange.Y}%";
                return $"Heal {amount} of maximum HP {method}";
            }
            return $"Heal {amount} HP {method}";
        }
    }
}