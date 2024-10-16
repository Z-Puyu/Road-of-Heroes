using System;

namespace Game.util.errors {
    public class SingletonException : Exception {
        public SingletonException(
            Type type
        ) : base($"{type} is meant to be a singleton class but a 2nd instance is being created!") {}
    }
}