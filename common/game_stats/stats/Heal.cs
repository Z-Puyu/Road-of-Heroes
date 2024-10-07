using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.stats {
    [RegisteredType(nameof(Heal), "", nameof(Resource)), GlobalClass]
    public partial class Heal : ModifiableValue {
        public ModifiableValueType TargetModifiableValue { set; get; }
        public Heal(ModifiableValueType targetModifiableValue, int amount) : base(targetModifiableValue switch {
            ModifiableValueType.Health => ModifiableValueType.HpHeal,
            ModifiableValueType.Magicka => ModifiableValueType.MagickaHeal,
            ModifiableValueType.Sanity => ModifiableValueType.SanityHeal,
            ModifiableValueType.Stamina => ModifiableValueType.StaminaHeal,
            _ => ModifiableValueType.HpHeal
        }, amount) {
            this.TargetModifiableValue = targetModifiableValue;
        }
    }
}