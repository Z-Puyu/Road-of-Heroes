using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace Game.common.effects.eot {
    [GlobalClass]
    public partial class MoTToken : Token {
        public override void Compose(EoT eot) {
            if (eot is MoT mot && mot.EffectType == this.Effect) {
                int expireTime = this.time + mot.TimeToLast;
                if (this.eots.TryGetValue(expireTime, out List<EoT> eots)) {
                    eots.Add(mot);
                } else {
                    this.eots.Add(expireTime, [mot]);
                }
                this.expiry = Math.Max(this.expiry, expireTime);
            }
        }

        public override Task Apply() {
            this.time += 1;
            return Task.CompletedTask;
        }
    }
}