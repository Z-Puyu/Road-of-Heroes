using System;

namespace Game.util.errors {
    public class EnumConversionException : Exception {
        public EnumConversionException(Enum from, Type to) 
                : base($"Cannot convert from {Enum.GetName(from.GetType(), from)} to {to}.") {}
    }
}