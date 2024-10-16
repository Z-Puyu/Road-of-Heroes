using System;
using Game.common.modifier;
using Game.util.enums;
using Game.util.errors;
using Godot;

namespace Game.common.stats {
    [GlobalClass]
    public partial class Stat : Resource {
        public StatType Type { set; get; } = StatType.Agility;
        protected int value = 0;
        protected Modifier Modifier { set; get; }

        [Export(PropertyHint.Range, "0,100")] 
        public int Value {
            get => this.Modifier.Modify(value);
            protected set => this.value = value;
        }

        public Stat() {
            this.Modifier = Modifier.IdentityOf(this.Type);
        }

        public Stat(StatType type, int value = 0) {
            this.Type = type;
            this.Value = value;
            this.Modifier = Modifier.IdentityOf(type);
        }

        public void AddModifier(ref Modifier modifier) {
            if (modifier.TargetStat == this.Type) {
                this.Modifier += modifier;
                modifier = this.Modifier;
            }
        }

        public override string ToString() {
            return $"{this.Value} {this.Type.ToText()}";
        }

        public static Stat operator+(Stat stat) {
            return stat;
        }

        public static Stat operator-(Stat stat) {
            return new Stat(stat.Type, -stat.value);
        }

        public static Stat operator+(Stat stat, int offset) {
            return new Stat(stat.Type, stat.value + offset);
        }

        public static Stat operator-(Stat stat, int offset) {
            return new Stat(stat.Type, stat.value - offset);
        }

        public static Stat operator*(Stat stat, double factor) {
            return new Stat(stat.Type, (int)Math.Round(stat.value * factor));
        }

        public static Stat operator/(Stat stat, double divisor) {
            return new Stat(stat.Type, (int)Math.Round(stat.value / divisor));
        }

        public static bool operator==(Stat s1, Stat s2) {
            return s1.Type == s2.Type && s1.Value == s2.Value;
        }

        public static bool operator!=(Stat s1, Stat s2) {
            return s1.Type != s2.Type || s1.Value != s2.Value;
        }

        public static bool operator>(Stat s1, Stat s2) {
            if (s1.Type != s2.Type) {
                throw new PartialOrderException<Stat, Stat>(s1, s2);
            }
            return s1.Value > s2.Value;
        }

        public static bool operator<(Stat s1, Stat s2) {
            if (s1.Type != s2.Type) {
                throw new PartialOrderException<Stat, Stat>(s1, s2);
            }
            return s1.Value < s2.Value;
        }

        public static bool operator>=(Stat s1, Stat s2) {
            return s1 == s2 || s1 > s2;
        }

        public static bool operator<=(Stat s1, Stat s2) {
            return s1 == s2 || s1 < s2;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj == null) {
                return false;
            }
            return obj is Stat s && this == s;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}