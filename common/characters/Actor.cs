using System;
using System.Linq;
using Game.common.characters.module;
using Game.common.modifier;
using Game.common.stats;
using Game.util;
using Godot;

namespace Game.common.characters {
    public abstract partial class Actor : Node, IComparable<Actor> {
        private readonly Guid id = Guid.NewGuid();
        private readonly StatModule statModule = new StatModule();
        private readonly ModifierModule modifierModule = new ModifierModule();
        private readonly CombatModule combatModule = new CombatModule();
        protected Character Data { get; set; }
        protected int SpeedOffset { set; get; } = 0;

        public Actor(Character data) {
            this.Data = data;
        }

        public override void _Ready() {
            foreach (Stat s in this.Data.Stats) {
                this.statModule.TryAdd(s);
            }
            this.AddChild(this.modifierModule);
            this.AddChild(this.statModule);
        }

        public static Actor Of(Character data) {
            if (data is Hero hero) {
                return new HeroActor(hero);
            }
            return new EnemyActor(data);
        }

        public Stat Get(StatType t) {
            if (this.statModule.TryGet(t, out Stat s)) {
                return this.modifierModule.Modify(s);
            }
            return this.modifierModule.Modify(new Stat(t, 0));
        }

        public abstract Stat Update(StatType t, int offset, int maxOffset = 0, int minOffset = 0);

        public void Attack(StatType dmgType, Actor target, bool crit, int multiplier) {
            // Send the modified damage to target.
            target.TakeDamage(this.modifierModule.Modify(this.combatModule.GenerateDamage(
                dmgType, this.Get(StatType.Strength).Value, crit, multiplier
            )));
        }

        public void Attack(Damage minDmg, Damage maxDmg, Actor target, bool crit) {
            // Send the modified damage to target.
            target.TakeDamage(this.modifierModule.Modify(this.combatModule.GenerateDamage(
                minDmg, maxDmg, crit
            )));
        }

        public void TakeDamage(Stat dmg) {
            this.Update(StatType.Health, -this.modifierModule.Modify(dmg).Value);
        }

        public void RandomiseSped() {
            this.SpeedOffset = Utilities.Randi(-3, 3);
        }

        public void ResetSpeed() {
            this.SpeedOffset = 0;
        }

        public int CompareTo(Actor other) {
            if (this.statModule.TryGet(StatType.Speed, out Stat value)) {
                if (other.statModule.TryGet(StatType.Speed, out Stat otherValue1)) {
                    return (otherValue1.Value + other.SpeedOffset).CompareTo(value.Value + this.SpeedOffset);
                }
                return other.SpeedOffset.CompareTo(value.Value + this.SpeedOffset);
            }
            if (other.statModule.TryGet(StatType.Speed, out Stat otherValue2)) {
                return (otherValue2.Value + other.SpeedOffset).CompareTo(this.SpeedOffset);
            }
            return other.SpeedOffset.CompareTo(this.SpeedOffset);
        }

        private partial class HeroActor : Actor {
            public HeroActor(Hero hero) : base(hero) {}

            public override Stat Update(StatType t, int offset, int maxOffset = 0, int minOffset = 0) {
                if (this.statModule.TryUpdate(t, out Stat s, offset, maxOffset, minOffset)) {
                    foreach (Stat stat in this.Data.Stats.Where(s => s.Type == t)) {
                        this.Data.Stats.Remove(stat);
                    }
                    this.Data.Stats.Add(s);
                    return s;
                }
                return null;
            }
        }   

        private partial class EnemyActor : Actor {
            public EnemyActor(Character data) : base(data) {}

            public override Stat Update(StatType t, int offset, int maxOffset = 0, int minOffset = 0) {
                if (this.statModule.TryUpdate(t, out Stat s, offset, maxOffset, minOffset)) {
                    foreach (Stat stat in this.Data.Stats.Where(s => s.Type == t)) {
                        this.Data.Stats.Remove(stat);
                    }
                    this.Data.Stats.Add(s);
                    return s;
                }
                return null;
            }
        }
    }
}