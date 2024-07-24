namespace Game.common.effects {
    public interface IEffectEmitter {
        public abstract int Emit(Effect.Type effect, int value);
    }
}