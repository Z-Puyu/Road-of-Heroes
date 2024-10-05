using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.common.characters;
using Game.common.modifier;
using Game.util;
using Game.util.events.battle;
using Godot;
using Godot.Collections;

namespace Game.common.effects.eot {
    [GlobalClass]
    public partial class TimedModifier : TimedEffect {
        private Actor Root { set; get; }

        public override async void _Ready() {
            await this.ToSignal(this.GetParent().GetParent(), SignalName.Ready);
            this.Root = this.GetParent().GetParent<Actor>();    
        }

        public override void Collect(EoT eot) {
            if (eot is MoT mot) {
                int expireTime = this.time + mot.TimeToLast;
                if (this.effects.TryGetValue(expireTime, out List<EoT> eots)) {
                    eots.Add(mot);
                } else {
                    this.effects.Add(expireTime, [mot]);
                }
                this.Publish(new ReceiveModifierEvent(this.Root, mot.Modifier));
            }
        }

        public override Task Apply(Actor target) {
            this.time += 1;
            if (this.effects.Remove(this.time, out List<EoT> list)) {
                if (this.effects.Count == 0) {
                    this.time = 0;
                }
                IEnumerable<Modifier> modifiers = [];
                foreach (MoT mot in list.Cast<MoT>()) {
                    modifiers = modifiers.Concat(mot.Modifier);
                }
                this.Publish(new RemoveModifierEvent(target, modifiers));
            }
            return Task.CompletedTask;
        }
    }
}