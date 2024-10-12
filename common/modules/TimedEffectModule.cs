using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.common.actions;
using Game.common.stats;
using Game.util.events;
using Game.util.events.system;
using Godot;

namespace Game.common.modules {
    [GlobalClass]
    public partial class TimedEffectModule : Module {
        private Dictionary<TimedEffect.Type, HashSet<TimedEffect>> TimedEffects { init; get; } = [];
        private Dictionary<int, Action> OnExpire { init; get; } = [];
        private int time = 0;

        public Stat Filter(Stat stat) {
            if (this.TimedEffects.TryGetValue(
                TimedEffect.Type.Buff, out HashSet<TimedEffect> buffs
            )) {
                foreach (Buff buff in buffs.Cast<Buff>()) {
                    stat = buff.Transform(stat);
                }
            }
            if (this.TimedEffects.TryGetValue(
                TimedEffect.Type.Debuff, out HashSet<TimedEffect> debuffs
            )) {
                foreach (Buff debuff in debuffs.Cast<Buff>()) {
                    stat = debuff.Transform(stat);
                }
            }
            return stat;
        }

        private void AddTimedEffect(TimedEffect effect) {
            int endTime = this.time + effect.Duration;
            if (this.TimedEffects.TryGetValue(
                effect.EffectType, out HashSet<TimedEffect> effects
            )) {
                effects.Add(effect);
            } else {
                effects = [effect];
                this.TimedEffects[effect.EffectType] = effects;
            }
            if (!this.OnExpire.ContainsKey(endTime)) {
                this.OnExpire[endTime] = () => effects.Remove(effect);
            } else {
                this.OnExpire[endTime] += () => effects.Remove(effect);
            }
        }

        protected override void ConnectEvents() {
            this.Subscribe<GameTickEvent>(this.OnGameTick);
        }

        private void OnGameTick(GameTickEvent @event) {
            this.time += 1;
            if (this.OnExpire.Remove(this.time, out Action onExpire)) {
                onExpire();
            }
            if (this.OnExpire.Count == 0) {
                this.time = 0;
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (TimedEffect.Type type in this.TimedEffects.Keys) {
                sb.AppendLine($"{type}: ");
                foreach (TimedEffect effect in this.TimedEffects[type]) {
                    sb.AppendLine(effect.ToString());
                }
            }
            return sb.ToString();
        }
    }
}