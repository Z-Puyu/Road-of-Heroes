using Game.common.stats;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.actions {
    [RegisteredType(nameof(Buff), "", nameof(Resource)), GlobalClass]
    public partial class Buff : TimedEffect {
        private bool isDebuff = false;
        [Export] public Array<TimedModifier> Modifiers { get; private set; } = [];
        [Export] private bool IsDebuff {
            get => isDebuff;
            set {
                isDebuff = value;
                if (isDebuff) {
                    EffectType = TimedEffect.Type.Debuff;
                } else {
                    EffectType = TimedEffect.Type.Buff;
                }
            }
        }

        [Export(PropertyHint.Range, "0,1000")] public override int Duration { get; protected set; }

        public Stat Transform(Stat stat) {
            foreach (TimedModifier modifier in this.Modifiers) {
                stat.AddModifier(modifier.Modifier);
            }
            return stat;
        }

        public Buff() : base(TimedEffect.Type.Buff, 1) {
            this.IsDebuff = false;
        }
    }
}