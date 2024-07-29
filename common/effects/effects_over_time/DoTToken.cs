using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.characters;
using Game.util;
using Godot;

namespace Game.common.effects.eot {
    [GlobalClass]
    public partial class DoTToken : Token {
        private readonly static Dictionary<EoT.Effect, Stat.Category> targetStats = new 
                Dictionary<EoT.Effect, Stat.Category> {
            {EoT.Effect.Bleed, Stat.Category.Health},
            {EoT.Effect.Burn, Stat.Category.Health},
            {EoT.Effect.Blight, Stat.Category.Magicka},
            {EoT.Effect.Frenzy, Stat.Category.Sanity},
            {EoT.Effect.Poison, Stat.Category.Health}
        };
        
        private int dmg = 0;

        public override void Compose(EoT eot) {
            if (eot is DoT dot && dot.EffectType == this.Effect) {
                this.dmg += dot.Amount;
                int expireTime = this.time + dot.TimeToLast;
                if (this.eots.TryGetValue(expireTime, out List<EoT> eots)) {
                    eots.Add(dot);
                } else {
                    this.eots.Add(expireTime, [dot]);
                }
                this.expiry = Math.Max(this.expiry, expireTime);
            }
        }

        protected override void Reset() {
            this.dmg = 0;
            base.Reset();
        }

        public override async Task Apply() {
            if (this.dmg <= 0) {
                return;
            }
            if (DoTToken.targetStats.TryGetValue(this.Effect, out Stat.Category stat)) {
                this.Root.Character.Update(stat, this.dmg);
                await this.Animate();
                await FloatingCaption.Node.Display(stat, this.dmg, this.Root.GlobalPosition);
            }
            this.time += 1;
            if (this.time == this.expiry) {
                this.Reset();
            }
            if (this.eots.TryGetValue(this.time, out List<EoT> eots)) {
                eots.ForEach(eot => {
                    if (eot is DoT dot) {
                        this.dmg -= dot.Amount;
                    } 
                });
                eots.Clear();
            }
        }
    }
}