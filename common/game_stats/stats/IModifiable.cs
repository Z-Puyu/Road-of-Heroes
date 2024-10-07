namespace Game.common.stats {
    public interface IModifiable {
        public abstract ModifiableValue ToModifiableValue();

        // public abstract T ToOriginal(ModifiableValue value);        
    }
}