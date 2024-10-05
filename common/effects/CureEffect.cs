using System.Threading.Tasks;
using Game.common.characters;
using Game.common.effects.eot;
using Game.common.tokens;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(CureEffect), "", nameof(Resource)), GlobalClass]
    public partial class CureEffect : Effect<Actor, Actor> {
        [Export] private OverTimeEffect TargetEffect { set; get; }

        public override async Task Apply(Actor src, Actor target, bool crit = false) {
            target.CureEffect(this.TargetEffect);
        }

        public override string ToDesc(Actor actor) {
            return this.ToString();
        }


        public override string ToString() {
            return $"Cure {this.TargetEffect}";
        }
    }
}