using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Game.util;
using Godot;

namespace Game.common.effects.eot {
    [GlobalClass]
    public partial class TimedDamage : TimedEffect {
        public override void Collect(EoT eot) {
            if (eot is DoT dot) {
                int expireTime = this.time + dot.TimeToLast;
                if (this.effects.TryGetValue(expireTime, out List<EoT> dots)) {
                    dots.Add(dot);
                } else {
                    this.effects.Add(expireTime, [dot]);
                }
            }
        }

        public override async Task Apply(Actor target) {
            int dmg = 0;
            foreach (List<EoT> dots in this.effects.Values) {
                foreach (DoT dot in dots.Cast<DoT>()) {
                    dmg += dot.Amount;
                }
            }
            if (dmg > 0) {
                switch (this.Effect) {
                    case OverTimeEffect.Blight:
                        target.Update(ModifiableValueType.Magicka, -dmg);
                        break;
                    case OverTimeEffect.Frenzy:
                        target.Update(ModifiableValueType.Sanity, -dmg);
                        break;
                    default:
                        target.Update(ModifiableValueType.Health, -dmg);
                        break;
                }   
            }
            base.Apply(target);
        }
    }
}