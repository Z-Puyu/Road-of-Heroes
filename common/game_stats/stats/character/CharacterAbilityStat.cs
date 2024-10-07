using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(CharacterAbilityStat), "", nameof(Resource)), GlobalClass]
    public partial class CharacterAbilityStat : GameValue {
        public static Array<CharacterAbilityStat> DEFAULT => [
            new CharacterAbilityStat(Type.Speed),
            new CharacterAbilityStat(Type.Strength),
            new CharacterAbilityStat(Type.Agility),
            new CharacterAbilityStat(Type.Perception),
            new CharacterAbilityStat(Type.Precision)
        ];
        
        private enum Type {
            Speed = ModifiableValueType.Speed,
            Strength = ModifiableValueType.Strength,
            Agility = ModifiableValueType.Agility,
            Perception = ModifiableValueType.Perception,
            Precision = ModifiableValueType.Precision   
        }
        [Export] private Type ValueType { get; set; }

        public CharacterAbilityStat() : base() {}

        private CharacterAbilityStat(Type type) : base() {
            this.ValueType = type;
        }
    }
}