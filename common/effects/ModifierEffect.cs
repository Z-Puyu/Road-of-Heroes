using Game.common.characters;
using Game.common.effects.eot;
using Game.util;
using Game.util.events.battle;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(ModifierEffect), "", nameof(Resource)), GlobalClass]
    public partial class ModifierEffect : Effect<Actor, Actor> {
        [Export] private MoT Buffs { set; get; }
        [Export] private MoT Debuffs { set; get; }
        [Export] private int SuccessChance { set; get; } = 100;

        public override void Apply(Actor src, Actor target, bool crit = false) {
            this.Publish(new ReceiveEffectEvent(
                this.Buffs, this.SuccessChance + (crit ? 50 : 0), target
            ));
            this.Publish(new ReceiveEffectEvent(
                this.Debuffs, this.SuccessChance + (crit ? 50 : 0), target
            ));
        }

        public override string ToDesc(Actor actor) {
            return this.ToString();
        }


        public override string ToString() {
            return $"{this.SuccessChance}% chance:\n" 
                 + $"{string.Join('\n', this.Buffs)}";
        }
    }
}