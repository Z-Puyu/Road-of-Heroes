using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.characters;
using Godot;

namespace Game.common.effects.eot {
    [GlobalClass]
    public partial class TimedModifier : TimedEffect {
        public override void Collect(EoT eot) {
            if (eot is MoT mot) {
                int expireTime = this.time + mot.TimeToLast;
                if (this.effects.TryGetValue(expireTime, out List<EoT> eots)) {
                    eots.Add(mot);
                } else {
                    this.effects.Add(expireTime, [mot]);
                }
            }
        }

        public override Task Apply(Actor target) {
            base.Apply(target);
            return Task.CompletedTask;
        }
    }
}