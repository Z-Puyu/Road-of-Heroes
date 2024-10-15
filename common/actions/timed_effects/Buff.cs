using Game.common.stats;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.actions {
    [RegisteredType(nameof(Buff), "", nameof(Resource)), GlobalClass]
    public partial class Buff : CombatEffect {
        private bool isDebuff = false;
        [Export] public Array<TimedModifier> Modifiers { get; private set; } = [];
        [Export] private bool IsDebuff {
            get => isDebuff;
            set {
                isDebuff = value;
                if (isDebuff) {
                    EffectType = CombatEffect.Type.Debuff;
                } else {
                    EffectType = CombatEffect.Type.Buff;
                }
            }
        }

        public Buff() : base(CombatEffect.Type.Buff) {
            this.IsDebuff = false;
        }
    }
}