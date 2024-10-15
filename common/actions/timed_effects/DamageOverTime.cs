using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions {
    [RegisteredType(nameof(DamageOverTime), "", nameof(Resource)), GlobalClass]
    public partial class DamageOverTime : CombatEffect {
        private enum DoT {
            Bleed = Type.Bleed, 
            Poison = Type.Poison, 
            Burn = Type.Burn, 
            Blight = Type.Blight, 
            Frenzy = Type.Frenzy
        }

        private DoT baseType;
        [Export] private DoT BaseType { 
            get => baseType; 
            set {
                baseType = value;
                EffectType = (Type)value;
            } 
        }
        [Export(PropertyHint.Range, "1,1000")] public int Damage { get; private set; }
        [Export(PropertyHint.Range, "0,1000")] public int Duration { get; protected set; } = 0;

        public DamageOverTime() : base(Type.Bleed) {}
    }
}