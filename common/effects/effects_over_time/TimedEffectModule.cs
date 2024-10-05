using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.effects;
using Game.common.effects.eot;
using Godot;

namespace Game.common.tokens {
	public partial class TimedEffectModule : Node {
		private readonly Dictionary<OverTimeEffect, TimedEffect> eots = [];

        public override void _Ready() {
            foreach (Node child in this.GetChildren()) {
                if (child is TimedEffect e) {
                    this.eots.Add(e.Effect, e);
                }
            }
        }

        public void Add(EoT eot) {
            this.eots[eot.EffectType].Collect(eot); 
        }

        public void Remove(OverTimeEffect effect) {
            this.eots[effect].Cure();
        }

        public async Task Apply() {
            foreach (TimedEffect e in this.eots.Values) {
                await e.Apply((Actor)this.GetParent());
            }
        }
    }
}
