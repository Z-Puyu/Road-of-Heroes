using System.Threading.Tasks;
using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.actions.combat {
    [RegisteredType(nameof(CombatAction), "", nameof(Resource)), GlobalClass]
    public abstract partial class CombatAction : Resource, IAction<Actor, Actor> {
        protected static bool ShouldChangeTarget(Actor src, Actor target) {
            return src.GetType() != target.GetType() && target.Warder != null;
        }

        public abstract Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None);

        public abstract string Describe(Actor src, Actor target);
    }
}