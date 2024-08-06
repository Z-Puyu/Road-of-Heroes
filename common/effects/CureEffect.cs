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
    public partial class CureEffect : Effect {
        private readonly static Dictionary<EoT.Effect, Stat.Category> resistance =
                new Dictionary<EoT.Effect, Stat.Category> {
            {EoT.Effect.Bleed, Stat.Category.BleedResist},
            {EoT.Effect.Blight, Stat.Category.BlightResist},
            {EoT.Effect.Burn, Stat.Category.BurnResist},
            {EoT.Effect.Poison, Stat.Category.PoisonResist},
            {EoT.Effect.Stun, Stat.Category.StunResist},
        };

        [Export] private EoT.Effect TargetEffect { set; get; }

        public override async Task Apply(IEffectEmitter src, IEffectReceiver target, bool crit = false) {
            if (target is not CharacterCard receiver || this.EffectType != Type.Cure) {
                return;
            }
            receiver.GetNode<EoTManager>(receiver.EoTManager).Remove(this.TargetEffect);
            await Task.CompletedTask;
        }

        public override string ToString() {
            return $"Cure {this.TargetEffect}";
        }
    }
}