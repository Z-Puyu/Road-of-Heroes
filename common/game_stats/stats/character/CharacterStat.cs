using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [GlobalClass]
    public partial class CharacterStat : CappedStat {
        public static readonly Array<CharacterStat> PLAYER_DEFAULT = [
            new CharacterStat(StatType.Health, 0),
            new CharacterStat(StatType.Magicka, 0),
            new CharacterStat(StatType.Sanity, 0),
            new CharacterStat(StatType.Stamina, 0)
        ];
        public static readonly Array<CharacterStat> ENEMY_DEFAULT = [
            new CharacterStat(StatType.Health, 0),
            new CharacterStat(StatType.Magicka, 0)
        ];

        private enum Category {
            HP = StatType.Health,
            Magicka = StatType.Magicka,
            Sanity = StatType.Sanity,
            Stamina = StatType.Stamina
        }

        private Category baseType;

        [Export] private Category BaseType { 
            get => baseType; 
            set {
                baseType = value;
                this.Type = (StatType)value;
                this.MaxValue.Type = value switch {
                    Category.HP => StatType.MaxHP,
                    Category.Magicka => StatType.MaxMagicka,
                    Category.Sanity => StatType.MaxSanity,
                    Category.Stamina => StatType.MaxStamina,
                    _ => StatType.MaxHP
                };
            } 
        }

        public CharacterStat() : base() {}

        public CharacterStat(StatType type, int value) : base(type, new CharacterAttribute(type switch {
            StatType.Health => StatType.MaxHP,
            StatType.Magicka => StatType.MaxMagicka,
            StatType.Sanity => StatType.MaxSanity,
            StatType.Stamina => StatType.MaxStamina,
            _ => StatType.MaxHP
        }, value)) {
            this.Type = type;
            this.BaseType = (Category)type;
        }
    }
}