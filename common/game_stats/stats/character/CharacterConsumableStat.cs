using Game.util.enums;
using Game.util.math;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(CharacterConsumableStat), "", nameof(Resource)), GlobalClass]
    public sealed partial class CharacterConsumableStat : CappedGameValue, IModifiable {
        public static Array<CharacterConsumableStat> DEFAULT => [
            new CharacterConsumableStat(Type.Health, 100),
            new CharacterConsumableStat(Type.Magicka, 100),
            new CharacterConsumableStat(Type.Sanity, 100),
            new CharacterConsumableStat(Type.Stamina, 100)
        ];

        private enum Type {
            Health = ModifiableValueType.Health,
            Magicka = ModifiableValueType.Magicka,
            Sanity = ModifiableValueType.Sanity,
            Stamina = ModifiableValueType.Stamina
        }

        public CharacterConsumableStat() : base() {}

        private CharacterConsumableStat(Type type, int value) : base() {
            this.ValueType = type;
            this.Value = value;
            this.MaxValue = value;
        }

        [Export] private Type ValueType { get; set; }

        public override string ToString() {
            return $"{this.ValueType}: {this.Value} / {this.MaxValue}";
        }

        public ModifiableValue ToModifiableValue() {
            return new ModifiableValue(
                this.ValueType.As<ModifiableValueType>(), this.Value, this.MaxValue
            );
        }
    }
}