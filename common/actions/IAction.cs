using System.Threading.Tasks;

namespace Game.common.actions {
    public interface IAction<S, T> {
        public abstract Task Apply(S src, T target, ActionFlag flag);
    }
}