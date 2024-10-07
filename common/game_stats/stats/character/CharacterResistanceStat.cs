using Game.util.enums;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(CharacterResistanceStat), "", nameof(Resource)), GlobalClass]
    public sealed partial class CharacterResistanceStat : GameValue, IModifiable {
        public static Array<CharacterResistanceStat> DEFAULT => [
            new CharacterResistanceStat(Type.BleedResistance),
            new CharacterResistanceStat(Type.PoisonResistance),
            new CharacterResistanceStat(Type.BurnResistance),
            new CharacterResistanceStat(Type.BlightResistance),
            new CharacterResistanceStat(Type.StunResistance)
        ];

        private enum Type {
            BleedResistance = ModifiableValueType.BleedResist,
            PoisonResistance = ModifiableValueType.PoisonResist,
            BurnResistance = ModifiableValueType.BurnResist,
            BlightResistance = ModifiableValueType.BlightResist,
            StunResistance = ModifiableValueType.StunResist,
        }

        [Export] private Type ValueType { get; set; }

        public CharacterResistanceStat() : base() {}

        private CharacterResistanceStat(Type type) : base() {
            this.ValueType = type;
        }

        public override string ToString() {
            return $"{this.ValueType}: {this.Value}%";
        }

        public ModifiableValue ToModifiableValue() {
            return new ModifiableValue(this.ValueType.As<ModifiableValueType>(), this.Value);
        }
    }
}