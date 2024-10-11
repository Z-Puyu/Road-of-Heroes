using System;
using System.Linq;
using Game.common.effects;
using Game.common.modules;
using Game.common.stats;
using Game.util;
using Game.util.math;
using Godot;

namespace Game.common.characters {
    public abstract partial class Actor : Node, IComparable<Actor> {
        private readonly Guid id = Guid.NewGuid();
        private readonly StatModule statModule = new StatModule();
        private readonly ModifierModule modifierModule = new ModifierModule();
        private readonly CombatModule combatModule = new CombatModule();
        private readonly TimedEffectModule timedEffectModule= new TimedEffectModule();
        protected Character Data { get; set; }
        protected int SpeedOffset { set; get; } = 0;

        public Actor(Character data) {
            this.Data = data;
        }

        public override void _Ready() {
            /* foreach (ModifiableValue s in this.Data.ModifiableValues) {
                this.statModule.TryAdd(s);
            } */
            this.AddChild(this.modifierModule);
            this.AddChild(this.statModule);
        }

        public static Actor Of(Character data) {
            if (data is Hero hero) {
                return new HeroActor(hero);
            }
            return new EnemyActor(data);
        }

        /// <summary>
        /// Computes a stat entry by applying the character's modifiers to <paramref name="stat"/>.
        /// </summary>
        /// <param name="stat">A stat entry containing the base data.</param>
        /// <returns>A stat entry containing the modified data.</returns>
        public ModifiableValue Filter(ModifiableValue stat) {
            return this.modifierModule.Modify(stat);
        }

        public ModifiableValue Get(ModifiableValueType t) {
            if (this.statModule.TryGet(t, out ModifiableValue s)) {
                return this.modifierModule.Modify(s);
            }
            return null; 
        }

        public abstract ModifiableValue Update(ModifiableValueType t, int offset, int maxOffset = 0, int minOffset = 0);

        public void RandomiseSped() {
            this.SpeedOffset = MathUtil.Randi(-3, 3);
        }

        public void ResetSpeed() {
            this.SpeedOffset = 0;
        }

        public int CompareTo(Actor other) {
            if (this.statModule.TryGet(ModifiableValueType.Speed, out ModifiableValue value)) {
                if (other.statModule.TryGet(ModifiableValueType.Speed, out ModifiableValue otherValue1)) {
                    return (otherValue1.Value + other.SpeedOffset).CompareTo(value.Value + this.SpeedOffset);
                }
                return other.SpeedOffset.CompareTo(value.Value + this.SpeedOffset);
            }
            if (other.statModule.TryGet(ModifiableValueType.Speed, out ModifiableValue otherValue2)) {
                return (otherValue2.Value + other.SpeedOffset).CompareTo(this.SpeedOffset);
            }
            return other.SpeedOffset.CompareTo(this.SpeedOffset);
        }

        private partial class HeroActor : Actor {
            public HeroActor(Hero hero) : base(hero) {}

            public override Stat Update(StatType t, int offset, int maxOffset = 0, int minOffset = 0) {
                if (this.statModule.TryUpdate(t, out Stat s, offset)) {
                    /* foreach (Stat stat in this.Data.Stats.Where(s => s.Type == t)) {
                        this.Data.Stats.Remove(stat);
                    }
                    this.Data.Stats.Add(s); */
                    return s;
                }
                return null;
            }
        }   

        private partial class EnemyActor : Actor {
            public EnemyActor(Character data) : base(data) {}

            public override Stat Update(StatType t, int offset, int maxOffset = 0, int minOffset = 0) {
                if (this.statModule.TryUpdate(t, out Stat s, offset)) {
                    /* foreach (Stat stat in this.Data.Stats.Where(s => s.Type == t)) {
                        this.Data.Stats.Remove(stat);
                    }
                    this.Data.Stats.Add(s); */
                    return s;
                }
                return null;
            }
        }
    }
}