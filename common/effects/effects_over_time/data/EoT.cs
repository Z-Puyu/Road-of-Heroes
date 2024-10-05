using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(EoT), "", nameof(Resource)), GlobalClass]
    public abstract partial class EoT : Resource {
        [Export] public OverTimeEffect EffectType { set; get; }
        [Export] public int TimeToLast { set; get; }        
    }
}