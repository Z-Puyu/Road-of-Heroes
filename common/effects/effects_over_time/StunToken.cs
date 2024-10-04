using System.Threading.Tasks;
using Godot;

namespace Game.common.effects.eot {
    [GlobalClass]
    public partial class StunToken : Token {
        public override void Compose(EoT eot) {
            if (eot is Stun stun && stun.EffectType == this.Effect) {
                int expireTime = this.time + 1;
                this.eots[expireTime] = [stun];
                this.expiry = expireTime;
            }
        }

        public override async Task Apply() {
            await this.Animate();
            this.time += 1;
            if (this.time == this.expiry) {
                this.Reset();
            }
        }
    }
}