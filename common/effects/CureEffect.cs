using Game.common.characters;
using Game.util;
using Game.util.events;
using Game.util.events.battle;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(CureEffect), "", nameof(Resource)), GlobalClass]
    public partial class CureEffect : CombatEffect {
        [Export] private OverTimeEffect TargetEffect { set; get; }

        public override void Apply(Actor src, Actor target, bool crit = false) {
            this.Publish(new CureDoTEvent(this.TargetEffect, target));
        }

        public override string GetDesc(Actor src, Actor target) {
            return this.ToString();
        }

        public override string ToString() {
            return $"Cure {this.TargetEffect}";
        }
    }
}