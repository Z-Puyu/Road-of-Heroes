using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Godot;

namespace Game.common.effects {
    public abstract partial class Effect<S, T> : Resource {
        protected Effectiveness Effectiveness { get; set; }
        public abstract Task Apply(S src, T target, bool crit = false);

        public abstract string ToDesc(Actor actor);
    }
}