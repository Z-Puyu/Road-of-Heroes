using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.common.actions;
using Game.common.characters;
using Game.common.modifier;
using Game.common.stats;
using Game.util.enums;
using Game.util.events;
using Game.util.events.battle;
using Game.util.events.characters;
using Game.util.events.system;
using Godot;

namespace Game.common.modules {
    [GlobalClass]
    public partial class CombatEffectModule : Module {
        private Dictionary<CombatEffect.Type, DoTRegistry> DoTs { init; get; } = [];
        private Dictionary<StatType, BuffRegistry> Buffs { init; get; } = [];
        private Dictionary<StatType, BuffRegistry> Debuffs { init; get; } = [];

        public Stat Filter(Stat stat) {
            if (this.Buffs.TryGetValue(stat.Type, out BuffRegistry buff)) {
                return buff.Transform(stat);
            }
            return stat;
        }

        public void Add(Actor src, CombatEffect effect) {
            switch (effect.EffectType) {
                case CombatEffect.Type.Buff:
                    foreach (TimedModifier m in ((Buff)effect).Modifiers) {
                        if (!this.Buffs.TryAdd(m.Modifier.TargetStat, new BuffRegistry(
                            m, () => this.Buffs.Remove(m.Modifier.TargetStat)
                        ))) {
                            this.Buffs[m.Modifier.TargetStat].Add((Actor)this.Root, m);
                        }
                    }
                    break;
                case CombatEffect.Type.Debuff:
                    foreach (TimedModifier m in ((Buff)effect).Modifiers) {
                        if (!this.Debuffs.TryAdd(m.Modifier.TargetStat, new BuffRegistry(
                            m, () => this.Debuffs.Remove(m.Modifier.TargetStat)
                        ))) {
                            this.Debuffs[m.Modifier.TargetStat].Add((Actor)this.Root, m);
                        }
                    }
                    break;
                case CombatEffect.Type.Bleed:
                case CombatEffect.Type.Blight:
                case CombatEffect.Type.Poison:
                case CombatEffect.Type.Burn:
                case CombatEffect.Type.Frenzy:
                    DamageOverTime dot = (DamageOverTime)effect;
                    if (!this.DoTs.TryAdd(dot.EffectType, new DoTRegistry(
                        dot, () => this.DoTs.Remove(dot.EffectType)
                    ))) {
                        this.DoTs[dot.EffectType].Add(dot);
                    }
                    break;
                case CombatEffect.Type.Stun:
                    this.Publish(new StunActorEvent(), [this.Root]);
                    break;
                case CombatEffect.Type.Stealth:
                    this.Publish(new StealthActorEvent(), [this.Root]);
                    break;
                case CombatEffect.Type.Warded:
                    this.Publish(new WardActorEvent(src), [this.Root]);
                    break;
                default:
                    break;
            }
        }

        public void Cure(CombatEffect.Type effect) {
            switch (effect) {
                case CombatEffect.Type.Buff:
                    this.Buffs.Clear();
                    break;
                case CombatEffect.Type.Debuff:
                    this.Debuffs.Clear();
                    break;
                case CombatEffect.Type.Bleed:
                case CombatEffect.Type.Blight:
                case CombatEffect.Type.Poison:
                case CombatEffect.Type.Burn:
                case CombatEffect.Type.Frenzy:
                    this.DoTs.Remove(effect);
                    break;
                default:
                    break;
            }
        }

        protected override void ConnectEvents(){
            this.Subscribe<GameTickEvent>(this.OnGameTick);
        }

        private void OnGameTick(GameTickEvent @event) {
            foreach (BuffRegistry buff in this.Buffs.Values) {
                buff.Tick((Actor)this.Root);
            }
            foreach (DoTRegistry dot in this.DoTs.Values) {
                dot.Tick((Actor)this.Root);
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (DoTRegistry dot in this.DoTs.Values) {
                sb.AppendLine(dot.ToString());
            }
            foreach (BuffRegistry buff in this.Buffs.Values) {
                sb.AppendLine(buff.ToString());
            }
            return sb.ToString();
        }

        private sealed class DoTRegistry {
            private CombatEffect.Type EffectType { init; get; } 
            private Dictionary<int, HashSet<DamageOverTime>> DoTs { init; get; } = [];
            private int Time { set; get; } = 0;
            private int Damage { set; get; } = 0;
            private Action OnEmpty { init; get; }

            public DoTRegistry(DamageOverTime init, Action onEmpty) { 
                this.EffectType = init.EffectType;
                this.DoTs = new Dictionary<int, HashSet<DamageOverTime>>{
                    {this.Time + init.Duration, [init]}
                };
                this.Damage = init.Damage;
                this.OnEmpty = onEmpty;
            }

            public void Add(DamageOverTime dot) {
                int endTime = this.Time + dot.Duration;
                if (!this.DoTs.TryAdd(endTime, [dot])) {
                    this.DoTs[endTime].Add(dot);
                }
                this.Damage += dot.Damage;
            }

            public void Tick(Actor actor) {
                this.Time += 1;
                StatType targetStat = this.EffectType switch {
                    CombatEffect.Type.Bleed => StatType.Health,
                    CombatEffect.Type.Poison => StatType.Health,
                    CombatEffect.Type.Blight => StatType.Magicka,
                    CombatEffect.Type.Burn => StatType.Health,
                    CombatEffect.Type.Frenzy => StatType.Sanity,
                    _ => StatType.Health
                };
                this.Publish(new UpdateStatsEvent(new Dictionary<StatType, int>{
                    {targetStat, -this.Damage}
                }));
                if (this.DoTs.Remove(this.Time, out HashSet<DamageOverTime> removed)) {
                    this.Damage -= removed.Sum(dot => dot.Damage);
                }
                if (this.DoTs.Count == 0) {
                    this.OnEmpty();
                }
            }

            public override string ToString() {
                return $"{this.Damage} × {this.DoTs.Keys.Max() - this.Time} {this.EffectType.ToText()} damage";
            }
        }

        private sealed class BuffRegistry {
            private StatType TargetStat { init; get; }  
            private Dictionary<int, Modifier> Modifiers { init; get; } = [];
            private int Time { set; get; } = 0;
            private Modifier OverallModifier { set; get; }
            private Action OnEmpty { init; get; }

            public BuffRegistry(TimedModifier init, Action onEmpty) {
                this.TargetStat = init.Modifier.TargetStat;
                this.Modifiers = new Dictionary<int, Modifier>{
                    {this.Time + init.Duration, init.Modifier}
                };
                this.OnEmpty = onEmpty;
                this.OverallModifier = Modifier.IdentityOf(this.TargetStat);
            }

            public void Add(Actor actor, TimedModifier m) {
                int endTime = this.Time + m.Duration;
                if (!this.Modifiers.TryAdd(endTime, m.Modifier)) {
                    this.Modifiers[endTime] += m.Modifier;
                }
                this.OverallModifier += m.Modifier;
                this.Publish(new ChangeModifierEvent(m.Modifier), [actor]);
            }

            public void Tick(Actor actor) {
                this.Time += 1;
                if (this.Modifiers.Remove(this.Time, out Modifier removed)) {
                    this.OverallModifier -= removed;
                    this.Publish(new ChangeModifierEvent(removed, true), [actor]);
                }
                if (this.Modifiers.Count == 0) {
                    this.OnEmpty();
                }
            }

            public Stat Transform(Stat stat) {
                Modifier modifier;
                foreach (Modifier m in this.Modifiers.Values) {
                    modifier = m;
                    stat.AddModifier(ref modifier);
                }
                return stat;
            }

            public bool IsIdentity() {
                return this.OverallModifier.IsIdentity();
            }

            public override string ToString() {
                return $"{this.OverallModifier} ({this.Modifiers.Keys.Max() - this.Time} turns)";
            }
        }
    }
}