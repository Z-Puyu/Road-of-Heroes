using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions {
    [RegisteredType(nameof(DamageOverTime), "", nameof(Resource)), GlobalClass]
    public partial class DamageOverTime : TimedEffect {
        private enum DoT {
            Bleed = TimedEffect.Type.Bleed, 
            Poison = TimedEffect.Type.Poison, 
            Burn = TimedEffect.Type.Burn, 
            Blight = TimedEffect.Type.Blight, 
            Frenzy = TimedEffect.Type.Frenzy
        }

        private DoT baseType;
        [Export] private DoT BaseType { 
            get => baseType; 
            set {
                baseType = value;
                EffectType = (TimedEffect.Type)value;
            } 
        }
        [Export(PropertyHint.Range, "1,1000")] public int Damage { get; private set; }
        [Export(PropertyHint.Range, "0,1000")] public override int Duration { get; protected set; }


        public DamageOverTime() : base(Type.Bleed, 1) {}
    }
}