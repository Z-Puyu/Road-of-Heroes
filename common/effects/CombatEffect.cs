using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(CombatEffect), "", nameof(Resource)), GlobalClass]
    public abstract partial class CombatEffect : Resource, IEffect<Actor, Actor> {
        public abstract void Apply(Actor src, Actor target, bool crit = false);
        public abstract string GetDesc(Actor src, Actor target);
    }
}