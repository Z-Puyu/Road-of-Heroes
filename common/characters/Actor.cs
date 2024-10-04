using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.common.modifier;
using Game.common.stats;
using Game.util;
using Godot;

namespace Game.common.characters {
    public partial class Actor : Node, IComparable<Actor> {
        private readonly Guid id = Guid.NewGuid();
        private readonly StatModule statModule = new StatModule();
        private readonly ModifierModule modifierModule = new ModifierModule();
        protected Character Data { get; set; }
        protected int SpeedOffset { set; get; } = 0;

        public Actor(Character data) {
            this.Data = data;
        }

        public override void _Ready() {
            foreach (Stat s in this.Data.Stats) {
                this.statModule.TryAdd(s.Type, s);
            }
            this.AddChild(this.modifierModule);
            this.AddChild(this.statModule);
        }

        public Stat Get(StatType t) {
            if (this.statModule.TryGet(t, out Stat s)) {
                return this.modifierModule.Modify(s);
            }
            return this.modifierModule.Modify(new Stat(t, 0));
        }

        public Stat Update(StatType t, int offset, int maxOffset = 0, int minOffset = 0) {
            if (this.statModule.TryUpdate(t, out Stat s, offset, maxOffset, minOffset)) {
                return s;
            }
            return null;
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
    }
}