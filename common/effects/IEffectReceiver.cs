namespace Game.common.effects {
    public interface IEffectReceiver {
        public abstract int Receive(Effect.Type effect, int value);        
    }
}