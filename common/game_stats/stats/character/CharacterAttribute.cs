using System;
using Godot;
using Godot.Collections;

namespace Game.common.stats {
    [GlobalClass]
    public partial class CharacterAttribute : Stat {
        public static readonly Array<CharacterAttribute> DEFAULT = [
            new CharacterAttribute(StatType.Agility, 0),
            new CharacterAttribute(StatType.Speed, 0),
            new CharacterAttribute(StatType.Strength, 0),
            new CharacterAttribute(StatType.Perception, 0),
            new CharacterAttribute(StatType.Precision, 0),
            new CharacterAttribute(StatType.BleedResist, 0),
            new CharacterAttribute(StatType.PoisonResist, 0),
            new CharacterAttribute(StatType.BurnResist, 0),
            new CharacterAttribute(StatType.BlightResist, 0),
            new CharacterAttribute(StatType.StunResist, 0)
        ];

        private enum Category {
            Agility = StatType.Agility,
            Speed = StatType.Speed,
            Strength = StatType.Strength,
            Perception = StatType.Perception,
            Precision = StatType.Precision,
            BleedResistance = StatType.BleedResist,
            PoisonResistance = StatType.PoisonResist,
            BurnResistance = StatType.BurnResist,
            BlightResistance = StatType.BlightResist,
            StunResistance = StatType.StunResist,
            MaxHP = StatType.MaxHP,
            MaxMagicka = StatType.MaxMagicka,
            MaxSanity = StatType.MaxSanity,
            MaxStamina = StatType.MaxStamina
        }

        private Category baseType;

        [Export] private Category BaseType { 
            get => baseType; 
            set {
                baseType = value;
                Type = (StatType)value;
            } 
        }

        public CharacterAttribute() : base() {}

        public CharacterAttribute(StatType type, int value) : base(type, value) {
            this.Type = type;
            this.BaseType = (Category)type;
        }

        public static CharacterAttribute operator+(CharacterAttribute stat) {
            return stat;
        }

        public static CharacterAttribute operator-(CharacterAttribute stat) {
            return new CharacterAttribute(stat.Type, -stat.value);
        }

        public static CharacterAttribute operator+(CharacterAttribute stat, int offset) {
            return new CharacterAttribute(stat.Type, stat.value + offset);
        }

        public static CharacterAttribute operator-(CharacterAttribute stat, int offset) {
            return new CharacterAttribute(stat.Type, stat.value - offset);
        }

        public static CharacterAttribute operator*(CharacterAttribute stat, double factor) {
            return new CharacterAttribute(stat.Type, (int)Math.Round(stat.value * factor));
        }

        public static CharacterAttribute operator/(CharacterAttribute stat, double divisor) {
            return new CharacterAttribute(stat.Type, (int)Math.Round(stat.value / divisor));
        }
    }
}