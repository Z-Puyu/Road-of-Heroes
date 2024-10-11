using System;
using System.Collections.Generic;
using Game.common.stats;
using Game.util.enums;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(Modifier), "", nameof(Resource)), GlobalClass]
    public partial class Modifier : Resource {
<<<<<<< HEAD
        private enum ValueType { Current, Max, Min }
        [Export] public int TimeToLast = 0;
        [Export] public bool UsePercentage { get; set; } = false;
        [Export] private int Value { set; get; } = 1;
        [Export] public ModifiableValueType TargetModifiableValue { get; set; }
        [Export] private ValueType TargetValue { get; set; } = ValueType.Current;
=======
        private static readonly Dictionary<StatType, Modifier> IDENTITY_MODIFIERS = [];

        [Export] private int Offset { set; get; } = 0;
        [Export(PropertyHint.Range, "-100,1000")] 
        private int Multiplier { set; get; } = 0;
        [Export] public StatType TargetStat { get; set; }
>>>>>>> e50a7f5edd12946b0af396b056629f5c7b368333

        public Modifier() {}

        public Modifier(StatType targetStat, int offset = 0, int multiplier = 0) {
            this.TargetStat = targetStat;
            this.Offset = offset;
            this.Multiplier = Math.Max(multiplier, -100);
        }

        public static Modifier IdentityOf(StatType t) { 
            if (Modifier.IDENTITY_MODIFIERS.TryGetValue(t, out Modifier m)) {
                return m;
            }
            Modifier mod = new Modifier(t);
            Modifier.IDENTITY_MODIFIERS[t] = mod;
            return mod;
        }

        public int Modify(int @base) {
            return (int)Math.Round(100 + this.Multiplier * (@base + this.Offset) / 100.0);
        }

        public override string ToString() {
            List<string> lines = [];
            if (this.Offset != 0) {
                lines.Add($"{this.TargetStat.ToText()} {this.Offset.ToSigned()}");
            }
            if (this.Multiplier != 0) {
                lines.Add($"{this.TargetStat.ToText()} {this.Multiplier.ToSigned()}");
            }
            return string.Join('\n', lines);
        }

        public static Modifier operator +(Modifier m) {
            return m;
        }

        public static Modifier operator -(Modifier m) {
            return new Modifier(m.TargetStat, -m.Offset, -m.Multiplier);
        }

        public static Modifier operator +(Modifier m1, Modifier m2) {
            if (m1.TargetStat == m2.TargetStat) {
                return new Modifier(m1.TargetStat, m1.Offset + m2.Offset, m1.Multiplier + m2.Multiplier);
            }
            return m1;
        }

        public static Modifier operator -(Modifier m1, Modifier m2) {
            if (m1.TargetStat == m2.TargetStat) {
                return new Modifier(m1.TargetStat, m1.Offset - m2.Offset, m1.Multiplier - m2.Multiplier);
            }
            return m1;
        }
    }
}