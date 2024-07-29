using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects.eot {
    [RegisteredType(nameof(EoT), "", nameof(Resource)), GlobalClass]
    public abstract partial class EoT : Resource {
        public enum Effect {
            Bleed,
            Blight,
            Poison,
            Burn,
            Frenzy,
            Stun,
            Buff,
            Debuff
        }

        [Export] public Effect EffectType { set; get; }
        [Export] public int TimeToLast { set; get; }        
    }
}