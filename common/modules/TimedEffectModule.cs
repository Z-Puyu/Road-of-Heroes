using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.effects;
using Game.util;
using Game.util.events.battle;
using Godot;

namespace Game.common.modules {
	public partial class TimedEffectModule : Node {
        private Actor Root { set; get; }
		private readonly Dictionary<OverTimeEffect, TimedEffect> eots = [];

        public override async void _Ready() {
            foreach (Node child in this.GetChildren()) {
                if (child is TimedEffect e) {
                    this.eots.Add(e.Effect, e);
                }
            }
            await this.ToSignal(this.GetParent(), SignalName.Ready);
            this.Root = this.GetParent<Actor>();
            this.Subscribe<ReceiveEffectEvent>(this.OnReceiveEffect);
            this.Subscribe<CureDoTEvent>(this.OnCure);
        }

        private void OnCure(object sender, EventArgs e) {
            if (e is CureDoTEvent @event && @event.HandledBy(this.Root)) {
                if (this.Remove(@event.Effect)) {
                    // Play animation
                }
            }
        }

        private void OnReceiveEffect(object sender, EventArgs e) {
            if (e is ReceiveEffectEvent @event && @event.HandledBy(this.Root)) {
                if (Util.Randi(1, 100) <= @event.Chance) {
                    this.Add(@event.Effect);
                }
            }
        }

        public void Add(EoT eot) {
            this.eots[eot.EffectType].Collect(eot); 
        }

        public bool Remove(OverTimeEffect effect) {
            return this.eots[effect].Cure();
        }

        public async Task Apply() {
            foreach (TimedEffect e in this.eots.Values) {
                await e.Apply((Actor)this.GetParent());
            }
        }
    }
}
