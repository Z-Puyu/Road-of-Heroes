using System;
using Game.util.errors;

namespace Game.util.enums {
    public static class EnumUtil {
        public static E As<E>(this Enum value) where E : Enum {
            if (Enum.IsDefined(typeof(E), value)) {
                return (E)value;
            }
            throw new EnumConversionException(value, typeof(E));
        }
    }
}