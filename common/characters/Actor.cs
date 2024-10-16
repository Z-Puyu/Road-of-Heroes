using System;
using System.Collections.Generic;
using System.Linq;
using Game.common.actions;
using Game.common.modifier;
using Game.common.modules;
using Game.common.stats;
using Game.util.events;
using Game.util.events.battle;
using Game.util.events.characters;
using Game.util.math;
using Godot;

namespace Game.common.characters {
    public abstract partial class Actor : Node, IComparable<Actor> {
        public Guid Id { get; private init; } = Guid.NewGuid();
        private StatModule StatModule { get; init; } = new StatModule();
        private CombatEffectModule CombatEffectModule { get; init; } = new CombatEffectModule();
        protected Character Data { get; set; }
        protected int SpeedOffset { set; get; } = 0;
        protected bool IsStunned { get; set; } = false;
        protected bool IsStealth { get; set; } = false;
        public Actor Warder { get; protected set; } = null;
        public Actor Ward { get; protected set; } = null;

        public Actor(Character data) {
            this.Data = data;
        }

        public override void _Ready() {
            foreach (Stat s in this.Data.Stats) {
                this.StatModule.TryAdd(s);
            }
            this.Subscribe<UpdateStatsEvent>(this.OnUpdateStats);
            this.Subscribe<ChangeModifierEvent>(this.OnChangeModifier);
            this.Subscribe<StunActorEvent>(e => this.IsStunned = true);
            this.Subscribe<StealthActorEvent>(e => this.IsStealth = true);
            this.Subscribe<WardActorEvent>(this.OnWardActor);
        }

        private void OnWardActor(WardActorEvent @event) {
            if (@event.Warder.Ward != null) {
                // If the warder is protecting someone else, withdraw the protection.
                @event.Warder.Ward.Warder = null;
            }
            if (@event.Warder.Warder != null) {
                // If the warder is being protected, withdraw the protection.
                @event.Warder.Warder = null;
                @event.Warder.Warder.Ward = null;
            }
            if (this.Ward != null) {
                // If you wish to be protected by someone else, you should not be protecting anyone.
                this.Ward.Warder = null;
                this.Ward = null;
            }
            if (this.Warder != null) {
                // If the actor is warded by someone else, change the warder.
                this.Warder.Ward = null;
            }
            this.Warder = @event.Warder;
            @event.Warder.Ward = this;
        }


        private void OnChangeModifier(ChangeModifierEvent @event) {
            Modifier modifier = @event.Modifier;
            if (@event.IsRemoving) {
                this.StatModule.TryRemoveModifier(ref modifier);
            } else {
                this.StatModule.TryAddModifier(ref modifier);
            }
        }

        private void OnUpdateStats(UpdateStatsEvent @event) {
            foreach (KeyValuePair<StatType, int> pair in @event.Changes) {
                this.Update(pair.Key, pair.Value);
            }
        }

        public static Actor Of(Character data) {
            if (data is Hero hero) {
                return new HeroActor(hero);
            }
            return new EnemyActor(data);
        }

        public void AddEffect(Actor src, CombatEffect effect) {
            this.CombatEffectModule.Add(src, effect);
        }

        public void Cure(CombatEffect.Type effect) {
            this.CombatEffectModule.Cure(effect);
        }

        /// <summary>
        /// Computes a stat entry by applying the character's buffs to <paramref name="stat"/>.
        /// </summary>
        /// <param name="stat">A stat entry containing the base data.</param>
        /// <returns>A stat entry containing the modified data.</returns>
        public Stat Filter(Stat stat) {
            return this.CombatEffectModule.Filter(stat);
        }

        public int Get(StatType t) {
            if (this.StatModule.TryGet(t, out int value)) {
                return value;
            }
            return 0; 
        }

        public abstract int Update(StatType t, int offset);

        public void RandomiseSped() {
            this.SpeedOffset = MathUtil.Randi(-3, 3);
        }

        public void ResetSpeed() {
            this.SpeedOffset = 0;
        }

        public int CompareTo(Actor other) {
            if (this.StatModule.TryGet(StatType.Speed, out int value)) {
                if (other.StatModule.TryGet(StatType.Speed, out int otherValue1)) {
                    return (otherValue1 + other.SpeedOffset).CompareTo(value + this.SpeedOffset);
                }
                return other.SpeedOffset.CompareTo(value + this.SpeedOffset);
            }
            if (other.StatModule.TryGet(StatType.Speed, out int otherValue2)) {
                return (otherValue2 + other.SpeedOffset).CompareTo(this.SpeedOffset);
            }
            return other.SpeedOffset.CompareTo(this.SpeedOffset);
        }

        private partial class HeroActor : Actor {
            public HeroActor(Hero hero) : base(hero) {}

            public override int Update(StatType t, int offset) {
                if (this.StatModule.TryUpdate(t, offset, out int value)) {
                    foreach (CharacterStat stat in this.Data.Stats.Where(s => s.Type == t)) {
                        this.Data.Stats.Remove(stat);
                        this.Data.Stats.Add(stat + offset);
                    }
                    foreach (CharacterAttribute stat in this.Data.Attributes.Where(s => s.Type == t)) {
                        this.Data.Attributes.Remove(stat);
                        this.Data.Attributes.Add(stat + offset);
                    }
                    return value;
                }
                return 0;
            }
        }   

        private partial class EnemyActor : Actor {
            public EnemyActor(Character data) : base(data) {}

            public override int Update(StatType t, int offset) {
                return 0;
            }
        }
    }
}