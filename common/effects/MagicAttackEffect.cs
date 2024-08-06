using System;
using System.Threading.Tasks;
using Game.common.characters;
using Game.util;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(MagicAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class MagicAttackEffect : Effect {
        [Export] private Vector2I DamageRange { set; get; } = Vector2I.Zero;

        public override async Task Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (src is not CharacterCard || target is not CharacterCard card) {
                return;
            }
            Character receiver = card.Character;
            int dmg = target.Receive(
                Type.MagicAttack, src.Emit(Type.MagicAttack, (int)Math.Round(
                    crit ? this.DamageRange.Y * 1.5 
                         : Utilities.Randi(this.DamageRange.X, this.DamageRange.Y)
                ))
            );
            await FloatingCaption.Node.Display(Stat.Category.Health, -dmg, card.GlobalPosition, crit);
            receiver.Update(Stat.Category.Health, -dmg);
        }

        public override string ToString() {
            return this.DamageRange.X == this.DamageRange.Y 
                    ? $"{this.DamageRange.X} magic damage" 
                    : $"{this.DamageRange.X} to {this.DamageRange.Y} magic damage";
        }
    }
}