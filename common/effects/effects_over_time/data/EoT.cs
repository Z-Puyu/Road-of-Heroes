using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects.eot {
    [RegisteredType(nameof(EoT), "", nameof(Resource)), GlobalClass]
    public abstract partial class EoT : Resource {
        public enum Effect {
            Bleed = Stat.Category.BleedResist,
            Blight = Stat.Category.BlightResist,
            Poison = Stat.Category.PoisonResist,
            Burn = Stat.Category.BurnResist,
            Frenzy = 0,
            Stun = Stat.Category.StunResist,
            Buff,
            Debuff
        }

        [Export] public Effect EffectType { set; get; }
        [Export] public int TimeToLast { set; get; }        
    }
}