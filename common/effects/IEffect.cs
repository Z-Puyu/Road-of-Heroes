using Game.common.characters;

namespace Game.common.effects {
    public interface IEffect<S, T> {
        public abstract string GetDesc(S src, T target);
    }
}