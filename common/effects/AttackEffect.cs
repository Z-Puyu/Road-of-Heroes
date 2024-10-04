using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(AttackEffect), "", nameof(Resource)), GlobalClass]
    public abstract class AttackEffect : Effect<Actor, Actor> {}
}